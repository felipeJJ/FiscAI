import fs from 'fs/promises';
import fetch from 'node-fetch';
import cliProgress from 'cli-progress';

// Caminhos dos arquivos e URL da API
const skuFile = 'lista_skuID_produtos.json';
const outputFile = 'dados_produtos.json';
const retryFile = 'sku_ids_retry.json';
const apiUrl = 'https://www."substituir".com.br/api/catalog_system/pub/products/search?fq=productId:';
const MAX_RETRIES = 3; // Número máximo de tentativas por SKU

// Função para buscar dados de um produto a partir de um SKU ID
async function fetchProductData(skuId, retryCount = 0) {
    const url = `${apiUrl}${skuId}`;
    const delay = Math.floor(Math.random() * (2000 - 1000 + 1)) + 1800; // Delay aleatório entre 200ms e 1000ms

    try {
        // Atraso antes de fazer a requisição para evitar sobrecarregar o servidor
        await new Promise(res => setTimeout(res, delay));

        const response = await fetch(url, {
            headers: {
                "accept": "*/*",
                "accept-language": "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7",
                "if-none-match": "\"25E49FB9D9EA4BD5827A07AF78EF16A0\"",
                "priority": "u=1, i",
                "sec-ch-ua": "\"Google Chrome\";v=\"129\", \"Not=A?Brand\";v=\"8\", \"Chromium\";v=\"129\"",
                "sec-ch-ua-arch": "\"arm\"",
                "sec-ch-ua-bitness": "\"64\"",
                "sec-ch-ua-full-version": "\"129.0.6668.101\"",
                "sec-ch-ua-full-version-list": "\"Google Chrome\";v=\"129.0.6668.101\", \"Not=A?Brand\";v=\"8.0.0.0\", \"Chromium\";v=\"129.0.6668.101\"",
                "sec-ch-ua-mobile": "?0",
                "sec-ch-ua-model": "\"\"",
                "sec-ch-ua-platform": "\"macOS\"",
                "sec-ch-ua-platform-version": "\"15.0.1\"",
                "sec-fetch-dest": "empty",
                "sec-fetch-mode": "cors",
                "sec-fetch-site": "same-origin",
                "Referrer-Policy": "strict-origin-when-cross-origin"
            },
            method: "GET",
        });

        if (response.status === 429) { // Limite de requisições atingido
            if (retryCount < MAX_RETRIES) {
                console.warn(`Limite de requisições excedido para SKU ID ${skuId}. Tentando novamente...`);
                return await fetchProductData(skuId, retryCount + 1);
            } else {
                console.error(`Limite de tentativas excedido para SKU ID ${skuId}. Pulando...`);
                return null;
            }
        }

        if (!response.ok) {
            console.error(`Erro ao buscar dados para SKU ID ${skuId}: ${response.statusText}`);
            return null;
        }

        const data = await response.json();

        if (data.length === 0) {
            console.log(`Nenhum produto encontrado para SKU ID: ${skuId}`);
            return 'empty'; 
        }

        const product = data[0];
        return {
            productId: product.productId,
            productName: product.productName,
            brand: product.brand,
            categories: product.categories || [],
            productClusters: product.productClusters || {},
            images: product.items?.[0]?.images?.map(image => image.imageUrl) || [],
            sellers: product.items?.[0]?.sellers?.map(seller => ({
                commertialOffer: {
                    Installment: seller.commertialOffer?.Installments?.[0]
                        ? { Value: seller.commertialOffer.Installments[0].Value }
                        : null
                }
            })) || [],
        };

    } catch (error) {
        console.error(`Erro ao buscar dados para SKU ID ${skuId}: ${error.message}`);
        return null;
    }
}

// Função principal para processar todos os SKU IDs e salvar em tempo real
async function processAllSkus(skuIds) {
    const failedSkus = []; // Lista para armazenar os SKU IDs que falharam

    // Carregar os dados já processados
    const existingData = await fs.readFile(outputFile, 'utf-8').catch(() => '[]'); // Lê o arquivo ou cria uma lista vazia
    const productsData = JSON.parse(existingData);
    const processedIds = new Set(productsData.map(product => product.productId)); // IDs já processados

    // Configuração da barra de progresso
    const progressBar = new cliProgress.SingleBar({
        format: 'Progresso |{bar}| {percentage}% | ETA: {eta_formatted} | Processados: {value}/{total}'
    }, cliProgress.Presets.shades_classic);
    progressBar.start(skuIds.length, 0);

    const startTime = Date.now();

    for (let i = 0; i < skuIds.length; i++) {
        const skuId = skuIds[i];

        // Pula o SKU se já foi processado
        if (processedIds.has(skuId)) {
            progressBar.update(i + 1);
            continue;
        }

        const result = await fetchProductData(skuId);

        if (result && result !== 'empty') {
            productsData.push(result);
            processedIds.add(skuId); // Marca o SKU como processado
            await fs.writeFile(outputFile, JSON.stringify(productsData, null, 4), 'utf-8');
        } else if (result === null) {
            failedSkus.push(skuId); // Adiciona o SKU ID à lista de falhas
        }

        // Atualiza a barra de progresso e exibe o tempo estimado em minutos
        const elapsedTime = (Date.now() - startTime) / 1000; // Tempo decorrido em segundos
        const estimatedTotalTime = (elapsedTime / (i + 1)) * skuIds.length; // Estimativa de tempo total
        const remainingTime = estimatedTotalTime - elapsedTime; // Tempo restante estimado em segundos
        const remainingTimeInMinutes = remainingTime / 60; // Converte o tempo restante para minutos
            progressBar.update(i + 1, {
            eta_formatted: `${remainingTimeInMinutes.toFixed(2)} min` // Formata para exibir com duas casas decimais
        });
    }

    progressBar.stop();

    if (failedSkus.length > 0) {
        await fs.writeFile(retryFile, JSON.stringify(failedSkus, null, 4), 'utf-8');
        console.log(`${failedSkus.length} SKUs falharam. IDs para tentar novamente salvos em '${retryFile}'.`);
    } else {
        console.log("Todos os SKU IDs foram processados com sucesso.");
    }
}

// Função para tentar novamente os SKUs que falharam
async function retryFailedSkus() {
    try {
        const retryData = await fs.readFile(retryFile, 'utf-8');
        const retrySkus = JSON.parse(retryData);

        if (retrySkus.length === 0) {
            console.log("Nenhum SKU para reprocessar.");
            return;
        }

        console.log(`Tentando novamente ${retrySkus.length} SKUs que falharam.`);
        await processAllSkus(retrySkus);

        // Exclui o arquivo de retry após o reprocessamento bem-sucedido
        await fs.unlink(retryFile);
        console.log(`Arquivo '${retryFile}' excluído.`);
    } catch (error) {
        console.error(`Erro ao tentar reprocessar SKUs: ${error.message}`);
    }
}

// Função principal para iniciar o processo
async function main() {
    const skuData = await fs.readFile(skuFile, 'utf-8');
    const skuIds = JSON.parse(skuData);

    await processAllSkus(skuIds);
    await retryFailedSkus();
}

main();
