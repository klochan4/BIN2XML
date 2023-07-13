using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Porovnavanie_suborov_generovanie_xml
{

    /// <summary>
    /// Trieda slúži na debugging a odosielanie prípadných chybových hlásení vývojárovi počas toho, 
    /// ako sa už daná aplikácia používa v praxi
    /// </summary>
    internal static class Debug
    {
        /// <summary>
        /// Sends string to developer and informs user of the program about the error via KryptonMessageBox
        /// </summary>
        /// <param name="exceptionMessage">string representation of exception</param>
        public static void informUserAndSendException(String exceptionMessage)
        {
            sendErrorMessageToDeveloper(exceptionMessage);
            KryptonMessageBox.Show(Constants.PROGRAM_ERROR + exceptionMessage);
        }


        /// <summary>
        /// Creates task to send error message to developer
        /// </summary>
        /// <param name="errorMessage">Message to be send to developer</param>
        public static void sendErrorMessageToDeveloper(string errorMessage)
        {
            Task t = Task.Run(() => sendErrMessage(errorMessage));
        }


        /// <summary>
        /// Creates httpclient and sends message to developer
        /// </summary>
        /// <param name="message">message to be send</param>
        private static async void sendErrMessage(String message)
        {
            try
            {
                HttpClient client = new HttpClient();

                int maxLengthOfMessage = 10000;
                int lastIndexOfStartOfSubstring = 0;

                while (lastIndexOfStartOfSubstring < message.Length)
                {
                    HttpContent content;
                    if (lastIndexOfStartOfSubstring + maxLengthOfMessage > message.Length)
                        content = new StringContent(message.Substring(lastIndexOfStartOfSubstring), Encoding.UTF8, "text/plain");
                    else
                        content = new StringContent(message.Substring(lastIndexOfStartOfSubstring, maxLengthOfMessage), Encoding.UTF8, "text/plain");

                    lastIndexOfStartOfSubstring += maxLengthOfMessage;
                    await Task.Run(() => client.PostAsync(new Uri(Constants.SERVER_FOR_ERRORS), content));
                    content.Dispose();
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                #warning log this to file instead
                //KryptonMessageBox.Show(Constants.PROGRAM_ERROR + ex.ToString());
            }
        }



        /// <summary>
        /// Sends to developer stacktrace, frame 1, field and properties and its values of referenceToObject object
        /// </summary>
        /// <param name="referenceToObject">reference object to generate stacktrace, frame1 field and properties</param>
        public static void generateErrorLog(Object referenceToObject)
        {
            try
            {
                string errorLog = "";

                var trace = new System.Diagnostics.StackTrace();

                errorLog += "Trace: " + trace + "\n";

                var frame = trace.GetFrame(1);

                errorLog += "Frame 1: " + frame + "\n";

                var methodName = frame.GetMethod().Name;

                errorLog += "Method name: " + methodName + "\n";

                var properties = referenceToObject.GetType().GetProperties(BindingFlags.NonPublic |
                                                            BindingFlags.Instance);
                var fields = referenceToObject.GetType().GetFields(BindingFlags.NonPublic |
                         BindingFlags.Instance); // public fields
                                                 // for example:



                errorLog += "Properties:\n";

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(referenceToObject, null);

                    if (value != null)
                        value = value.ToString();

                    errorLog += prop + ": " + value + "\n";
                }


                errorLog += "Fields:\n";
                foreach (var field in fields)
                {
                    var value = field.GetValue(referenceToObject);

                    if (value != null)
                        value = value.ToString();

                    errorLog += field + ": " + value + "\n";
                }



                sendErrorMessageToDeveloper(errorLog);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(Constants.PROGRAM_ERROR + ex.ToString());
            }

        }


    }
}
