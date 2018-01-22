using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BalanceReaderEmulator
{
    public partial class BRForm : Form{

        private BalanceReaderFactory balanceReaderFactory; 
        private String ReadFrom;

        public BRForm(){
            InitializeComponent();
            CreateReadersList();
        }

        private void CreateReadersList(){
            balanceReaderFactory = new BalanceReaderFactory();
            ReadersList.Items.Clear();
            var readers = balanceReaderFactory.AvailableReaders;

            foreach (var reader in readers){
                ReadersList.Items.Add(reader);
            }

            if(readers.Length > 0){
                ReadersList.SelectedItem = readers[0];
                readButton.Enabled = true;
            }else{
                readButton.Enabled = false;
            }
        }

        private void ReadersListSelectedIndexChanged(object sender, EventArgs e){
            ReadFrom = ReadersList.SelectedItem.ToString();
        }

        private void readButtonClick(object sender, EventArgs e){
            readButton.Enabled = false;
            apdusTextBox.Text = "";

            new Thread(() =>{
                Thread.CurrentThread.IsBackground = true;
                try{
                    using (var balanceReader = balanceReaderFactory.CreateBalanceReader(ReadFrom))
                    {
                        balanceReader.NewRecordEventHandlers += OnNewRecord;
                        balanceReader.Run();
                    }
                }catch (Exception exception){
                    apdusTextBox.Invoke(new Action(delegate (){
                        apdusTextBox.Text = exception.Message;
                    }));
                }

                readButton.Invoke(new Action(delegate (){
                    readButton.Enabled = true;
                }));
            }).Start();
        }

        private void constTextBoxEnter(object sender, EventArgs e){
            ActiveControl = null;
        }

        private void refreshButtonClick(object sender, EventArgs e){
                CreateReadersList();
        }

        private void OnNewRecord(object sender, EventArgs eventArgs){
            apdusTextBox.Invoke(new Action(delegate () {
                apdusTextBox.AppendText(eventArgs.ToString());
                apdusTextBox.AppendText(Environment.NewLine);
            }));
        }
    }
}
