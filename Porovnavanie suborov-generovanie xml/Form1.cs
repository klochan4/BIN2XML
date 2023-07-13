using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Porovnavanie_suborov_generovanie_xml
{
    /// <summary>
    /// Autor: Bc. Andrej Klocháň
    /// email: kloky777@gmail.com
    /// 
    /// Program slúži na načítanie dvoch binárnych súborov, nájdenie rozdielov medzi nimi a vygenerovanie
    /// XML súboru (vo formáte vhodnom pre Batch Hex editor) pozostávajúceho z nájdených rozdielov.
    /// </summary>
    /// 


    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }


    public partial class Form1 : KryptonForm
    {
        public Form1()
        {
            //inicializácia grafických komponentov
            InitializeComponent();
        }

        
        /// <summary>
        /// Kliknutie tlačidla otvorí dialóg pre voľbu 1. súboru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            openFileDialog(ref tb_pathToFile1);
        }


        /// <summary>
        /// Kliknutie tlačidla otvorí dialóg pre voľbu 2. súboru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            openFileDialog(ref tb_pathToFile2);
        }


        public bool byteArraysEqual(byte[] x, byte[] y) {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            if (x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }


        int minimum_difference_length_in_bytes = 5;
        const int maximum_space_between_differences_in_bytes = 3;


        /// <summary>
        /// Otvorí dialóg na zvolenie súboru a v prípade úspechu nastaví textbox
        /// </summary>
        /// <param name="tb">Textbox, do ktorého sa uloží zvolená cesta k súboru</param>
        private void openFileDialog(ref KryptonTextBox tb) {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    
                    if(File.Exists(tb.Text))
                        if (Directory.Exists(Path.GetDirectoryName(tb.Text)))
                            openFileDialog.InitialDirectory = Path.GetDirectoryName(tb.Text);
                        else
                            openFileDialog.InitialDirectory = "c:\\";

                    openFileDialog.Filter = "Binárny súbor (*.bin)|*.bin|Všetky súbory (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        tb.Text = openFileDialog.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.generateErrorLog(this);
                Debug.informUserAndSendException(ex.ToString());
            }
        }



        /// <summary>
        /// Spustí spracovanie binárnych súborov a generovanie XML súboru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonButton2_Click(object sender, EventArgs e)
        {

            if (tb_pathToFile1.Text.Equals(String.Empty)) {
                KryptonMessageBox.Show("Je potrebné zvoliť súbor 1");
                return;
            }

            if (!File.Exists(tb_pathToFile1.Text)) {
                KryptonMessageBox.Show("Súbor 1 nebol nájdený");
                return;
            }

            if (tb_pathToFile2.Text.Equals(String.Empty))
            {
                KryptonMessageBox.Show("Je potrebné zvoliť súbor 2");
                return;
            }

            if (!File.Exists(tb_pathToFile2.Text))
            {
                KryptonMessageBox.Show("Súbor 2 nebol nájdený");
                return;
            }

            
            if (String.Equals(tb_pathToFile1.Text, tb_pathToFile2.Text)) {
                KryptonMessageBox.Show("Je potrebné zvoliť rozdielne súbory");
                return;
            }

            if (tb_outputFolder.Text.Equals(String.Empty))
            {
                KryptonMessageBox.Show("Je potrebné zvoliť výstupný priečinok");
                return;
            }

            if (!Directory.Exists(tb_outputFolder.Text))
            {
                KryptonMessageBox.Show("Cesta k výstupnému priečinku nebola nájdená");
                return;
            }

            if (!IsDirectoryWritable(tb_outputFolder.Text)) {
                KryptonMessageBox.Show("Do zvoleného priečinka nie je možné zapisovať. " +
                    "Zvoľte iný výstupný priečinok.");
                return;
            }


            BinaryReader file1_binReader = null;
            BinaryReader file2_binReader = null;


            try
            {
                file1_binReader = new BinaryReader(File.Open(tb_pathToFile1.Text, FileMode.Open));
                file2_binReader = new BinaryReader(File.Open(tb_pathToFile2.Text, FileMode.Open));

                if (file1_binReader.BaseStream.Length != file2_binReader.BaseStream.Length)
                {
                    KryptonMessageBox.Show("Zvolené súbory majú rozdielnu veľkosť. Vyhľadávanie rozdielov bolo zrušené");
                    return;
                }

                //tieto bajty sú vyhľadané - predstavujú jediný rozdiel,
                //teda v XML súbore jednu položku SSearchBytes
                List<byte> currentSearchBytes = new List<byte>();

                //tieto bajty budú prepísané bajtmi v zozname currentSearchBytes
                //- predstavujú jediný rozdiel, teda v XML súbore jednu položku SReplaceBytes
                List<byte> currentReplaceBytes = new List<byte>();

                List<byte[]> listOfAllSearchBytes = new List<byte[]>(); //tento zoznam obsahuje všetky nájdené rozdiely, teda sú tu všetky currentSearchBytes
                //resp. v XML súbore to sú všetky položky SSearchBytes

                List<byte[]> listOfAllReplaceBytes = new List<byte[]>(); //tu je viacero replace bytov, teda tieto rozdiely budú prepísané rozdielmi v listOfAllSearchBytes
                //resp. v XML súbore to sú všetky položky SReplaceBytes


                //tu prebieha vyhľadávanie rozdielov
                //for (long byteIndex = 0; byteIndex < file1_binReader.BaseStream.Length; byteIndex++)
                while (file1_binReader.BaseStream.Position < file1_binReader.BaseStream.Length)
                {
                    byte oneSearchByte = file1_binReader.ReadByte();
                    byte oneReplaceByte = file2_binReader.ReadByte();

                    bool oneByteAlreadyAdded = false;

                    //ak sa našiel rozdielny bit ALEBO ak sa našiel rovnaký bit avšak rozdiel ešte nebol
                    //uložený medzi zoznam search bytes a replace bytes a zároveň nájdených rozdielnych bajtov je menší ako minimálna dĺžka rozdielu
                    if (oneSearchByte != oneReplaceByte || (currentSearchBytes.Count < minimum_difference_length_in_bytes && currentSearchBytes.Count > 0))
                    {
                        //tak sa pridá ďalší bajt k rozdielom, aj keby mal byť oneSearchByte a oneReplaceByte rovnaký
                        currentSearchBytes.Add(oneSearchByte);
                        currentReplaceBytes.Add(oneReplaceByte);
                        oneByteAlreadyAdded = true;
                    }


                    //ak počet nájdených bajtov je väčší rovný ako minimálna dĺžka rozdielu a zároveň už sú posledné bajty rovnaké,
                    //ALEBO našli sa nejaké rozdielne bajty a aj keď ich počet nie je väčší/rovný ako minimálna dĺžka a zároveň vyhľadávanie sa dostalo na koniec súboru
                    if ((currentSearchBytes.Count >= minimum_difference_length_in_bytes && (oneSearchByte == oneReplaceByte)) || (currentSearchBytes.Count > 0 && (file1_binReader.BaseStream.Position == file1_binReader.BaseStream.Length)))
                    {
                        //príznak, ktorý sa nastaví na true, ak je medzera medzi rozdielmi väčšia ako definovaná hodnota,
                        //tým pádom sa ukončí pridávanie current search/replace bajtov do zoznamov všetkých search/replace bajtov
                        //ak sa nenastaví na true, tak nastáva preklenutie medzery a teda spojenie dvoch rozdielov
                        bool finishAddingDifference = false;

                        if (file1_binReader.BaseStream.Position == file1_binReader.BaseStream.Length && currentSearchBytes.Count < minimum_difference_length_in_bytes)
                        {
                            KryptonMessageBox.Show(String.Format("POZOR: Nájdená dvojica [  {0}  a  {1}  ] nachádzajúca sa na konci súborov nemá minimálnu dĺžku " +
                                "{2} hexbitov. Napriek tomu je vložená do XML súboru", Convert.ToBase64String(currentSearchBytes.ToArray()),
                                Convert.ToBase64String(currentReplaceBytes.ToArray()), minimum_difference_length_in_bytes));
                            finishAddingDifference = true;
                        }
                        else
                        {

                            long peekBytesLength;

                            //zistenie kolko bajtov mozno nacitat, aby nedoslo k preteceniu binreadera
                            if (file1_binReader.BaseStream.Position < file1_binReader.BaseStream.Length - maximum_space_between_differences_in_bytes)
                            {
                                
                                peekBytesLength = maximum_space_between_differences_in_bytes;
                            }
                            else
                            {
                                peekBytesLength = file1_binReader.BaseStream.Length - file1_binReader.BaseStream.Position;
                            }


                            if (peekBytesLength > 0)
                            {
                                byte[] peekBytes_file1 = file1_binReader.ReadBytes((int)peekBytesLength);
                                byte[] peekBytes_file2 = file2_binReader.ReadBytes((int)peekBytesLength);

                                if (peekBytes_file1.SequenceEqual<byte>(peekBytes_file2))
                                {
                                    finishAddingDifference = true;
                                }
                                else
                                {
                                    if (!oneByteAlreadyAdded) {
                                        currentSearchBytes.Add(oneSearchByte);
                                        currentReplaceBytes.Add(oneReplaceByte);
                                    }

                                    
                                    int differenceEndsAtIndex = 0;

                                    for (int i = 0; i < peekBytes_file1.Length; i++) {
                                        if (peekBytes_file1[i] != peekBytes_file2[i]) {
                                            differenceEndsAtIndex = i;
                                            break;
                                        }
                                    }

                                    {
                                        //pridanie len tej casti pola, ktora na poslednom mieste obsahuje rozdiel
                                        currentSearchBytes.AddRange(peekBytes_file1.SubArray(0, differenceEndsAtIndex + 1));
                                        currentReplaceBytes.AddRange(peekBytes_file2.SubArray(0, differenceEndsAtIndex + 1));
                                        file1_binReader.BaseStream.Position -= (peekBytesLength - differenceEndsAtIndex - 1);
                                        file2_binReader.BaseStream.Position -= (peekBytesLength - differenceEndsAtIndex - 1);
                                    }
                                }
                            }
                            else {
                                finishAddingDifference = true;
                            }
                        }

                        if (file1_binReader.BaseStream.Position == file1_binReader.BaseStream.Length && currentSearchBytes.Count > 0) {
                            finishAddingDifference = true;
                        }

                        if (currentSearchBytes.Count != currentReplaceBytes.Count)
                        {
                            KryptonMessageBox.Show("Niektorá z nájdených rozdielnych dvojíc nemá rovnaký počet bajtov\n" +
                                "XML súbor nebolo možné vygenerovať.");
                            return;
                        }


                        if (finishAddingDifference)
                        {
                            //pridá sa súčasné search bajty a replace bajty do zoznamu všetkých search resp. replace bajtov
                            listOfAllSearchBytes.Add(currentSearchBytes.ToArray());
                            listOfAllReplaceBytes.Add(currentReplaceBytes.ToArray());

                            //vynuluje sa zoznam súčasných search/replace bajtov a môže prebiehať vyhľadávanie nových rozdielov
                            currentSearchBytes.Clear();
                            currentReplaceBytes.Clear();
                        }

                    }


                }

                if (listOfAllSearchBytes.Count != listOfAllReplaceBytes.Count)
                {

                    KryptonMessageBox.Show("Bol nájdený rozdielny počet rozdielnych dvojíc\n" +
                        "XML súbor nebolo možné vygenerovať");
                    return;
                }


                string serialiableActionSequenceItems = String.Empty;


                //tu prebieha generovanie časti XML súboru - všetkých položiek SerializableActionSequenceItem
                for (int i = 0; i < listOfAllSearchBytes.Count; i++)
                {
                    if (listOfAllSearchBytes[i].Length != listOfAllReplaceBytes[i].Length)
                    {
                        KryptonMessageBox.Show("Nejaká dvojica searchbytes - replacebytes obsahuje rozdielny počet bajtov\n" +
                        "XML súbor nebolo možné vygenerovať");
                        return;
                    }

                    string SerializableActionSequenceItem = String.Format("<SerializableActionSequenceItem>\r\n      <InUse>true</InUse>\r\n      <Level>0</Level>\r\n      <ActionType>BinaryMark.aBinReplace</ActionType>\r\n      <ActionAssembly />\r\n      <Comment />\r\n      <ActionOptions type=\"BinaryMark.aBinReplaceOptions\" assembly=\"\">\r\n        <aBinReplaceOptions xmlns=\"\">\r\n          <SearchMatchPlaceholderInReplaceBytes>false</SearchMatchPlaceholderInReplaceBytes>\r\n          <WildCards>false</WildCards>\r\n          <WildCardByte>0</WildCardByte>\r\n          <RepWildCards>false</RepWildCards>\r\n          <RepWildCardByte>0</RepWildCardByte>\r\n          <FirstMatch>1</FirstMatch>\r\n          <ReplaceCount>0</ReplaceCount>\r\n          <Range>\r\n            <From>1</From>\r\n            <To>0</To>\r\n            <FromEnd>false</FromEnd>\r\n          </Range>\r\n          <SSearchBytes>{0}</SSearchBytes>\r\n          <SReplaceBytes>{1}</SReplaceBytes>\r\n        </aBinReplaceOptions>\r\n      </ActionOptions>\r\n    </SerializableActionSequenceItem>", Convert.ToBase64String(listOfAllSearchBytes[i]), Convert.ToBase64String(listOfAllReplaceBytes[i]));
                    serialiableActionSequenceItems += SerializableActionSequenceItem + "\r\n";
                }


                string outerStructure1 = "<?xml version=\"1.0\"?>\r\n<SerializableActionSequence xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"SASTemplate\">\r\n  <List>";
                string outerStructure2 = "</List>\r\n  <Name>SAS</Name>\r\n</SerializableActionSequence>";

                //tu prebieha vloženie serialiableActionSequenceItems do vonkajšej štruktúry XML súboru
                string outputString = outerStructure1 + serialiableActionSequenceItems + outerStructure2;


                //vytvorenie cesty k výstupnému súboru
                string path_outputFile = tb_outputFolder.Text + "\\" +
                    Path.GetFileNameWithoutExtension(tb_pathToFile1.Text) +
                    Path.GetFileNameWithoutExtension(tb_pathToFile2.Text) + ".xml";
                string path_oldOutputFile = path_outputFile;


                int j = 1;
                //ak už existuje výstupný súbor s rovnakým názvom, tak sa pridá prípona (1).xml, príp. vyššie
                while (File.Exists(path_outputFile))
                {
                    path_outputFile = path_oldOutputFile;
                    string[] splittedFilename = path_outputFile.Split('.');

                    splittedFilename[0] += " (" + j++ + ")";

                    path_outputFile = splittedFilename[0] + '.' + splittedFilename[1];
                }

                //zápis do XML súboru
                File.WriteAllText(path_outputFile, outputString);

                KryptonMessageBox.Show(String.Format("XML súbor {0} bol úspešne vygenerovaný", path_outputFile));
            }
            catch (System.IO.IOException ex) {
                KryptonMessageBox.Show(ex.Message);
            }
            finally
            {

                if (file1_binReader != null)
                {
                    file1_binReader.Close();
                    file1_binReader.Dispose();
                }


                if (file2_binReader != null)
                {
                    file2_binReader.Close();
                    file2_binReader.Dispose();
                }

            }
            
        }


        /// <summary>
        /// Checks if directory is writable by this program
        /// </summary>
        /// <param name="dirPath">Path to directory</param>
        /// <param name="throwIfFails">If true, methods rethrow exceptions if any</param>
        /// <returns></returns>
        public bool IsDirectoryWritable(string dirPath)
        {

            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch (Exception ex)
            {
                Debug.generateErrorLog(this);
                Debug.sendErrorMessageToDeveloper("dirPath: " + dirPath);
                Debug.informUserAndSendException(ex.ToString());
                return false;
            }
        }



        /// <summary>
        /// Po kliknutí na tlačidlo sa otvorí dialóg na voľbu výstupného priečinku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string selectPath = fbd.SelectedPath;

                        if (IsDirectoryWritable(selectPath))
                        {
                            tb_outputFolder.Text = fbd.SelectedPath;
                        }
                        else
                        {
                            KryptonMessageBox.Show("Program nemá prístup k zvolenému priečinku");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.generateErrorLog(this);
                Debug.informUserAndSendException(ex.ToString());

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //vycentruje tlačidlo
            btn_generateXML.Left = (this.ClientSize.Width - btn_generateXML.Width) / 2;
        }
    }
}
