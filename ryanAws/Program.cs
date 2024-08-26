using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.DynamoDBv2;

namespace Main
{
    public class Program
    {
        public static async Task Main()
        {
            // Prompt user for input
            await SendMessageToSqsAsync("test1");
        }

        private static async Task SendMessageToSqsAsync(string message)
        {
            try
            {
                // Send message to SQS
                var sendMessageResponse = await new AmazonSQSClient(RegionEndpoint.USWest2).SendMessageAsync(new SendMessageRequest
                {
                    QueueUrl = "https://sqs.us-west-2.amazonaws.com/248189899700/ryanAWS",
                    MessageBody = message
                });
                Console.WriteLine("Message ID: " + sendMessageResponse.MessageId);
            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine($"AWS SQS Error: {ex.Message}, Status Code: {ex.StatusCode}, Error Code: {ex.ErrorCode}");
            }
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine($"AWS DynamoDB Error: {ex.Message}, Status Code: {ex.StatusCode}, Error Code: {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
