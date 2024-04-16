using Azure.Storage.Blobs;

namespace WebAPI.Utils.BlobStorage
{
    public static class AzureBlobStorageHelper
    {

        public static async Task<string> UploadImageBlobAsync(IFormFile arquivo, string stringConexao, string nomeContainer)
        {
            try
            {
                // verifica se existe um arquivo 
                if (arquivo != null)
                {
                    // gera um nome único + extensão do arquivo 
                    var blobName = Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(arquivo.FileName);

                    //cria uma insistência do client Blob Service e passa a string de conexão 
                    var blobServiceClient = new BlobServiceClient(stringConexao);

                    //obtem um container client usando o nome do container do blob 
                    var blobContainerClient = blobServiceClient.GetBlobContainerClient(nomeContainer);

                    // obtem um blob client usando o blob name 
                    var blobClient = blobContainerClient.GetBlobClient(blobName);

                    // abre o fluxo de entrada do arquivo 
                    using (var stream = arquivo.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    // retorna a uri do blob como uma string 
                    return blobClient.Uri.ToString();
                }
                else
                {
                    // retorna a uri de uma imagem padrão caso nenhum arquivo seja enviado 
                    return "https://blobvitalhubgabrielg2t.blob.core.windows.net/containergabrielvitalhubg2t/avatar.jpg";

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
