using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MD5_Implementation_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public List<string> ListContent { get; set; }
        public MainWindow()
        {
            ListContent = new List<string>();
            InitializeComponent();
        }

        private void btnCommand_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Tag.Equals("Close"))
            {
                HandyControl.Controls.Dialog.Show(new DialogExit());
            }
            if (btn.Tag.Equals("Maximize"))
            {
                if (this.WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else WindowState = WindowState.Maximized;
            }
            if (btn.Tag.Equals("Minimize"))
            {
                if (this.WindowState == WindowState.Minimized)
                    WindowState = WindowState.Normal;
                else WindowState = WindowState.Minimized;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxPlaintext.Text))
            {
                ListContent.Clear();
                lbxContent.ItemsSource = null;
                lbxContent.Items.Clear();

                GenerateTemplate(tbxPlaintext.Text);
                lbxContent.ItemsSource = ListContent;
            }
        }

        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        private void GenerateTemplate(string plaintext)
        {
            byte[] input = Encoding.ASCII.GetBytes(plaintext);

            ListContent.Add($"{plaintext} (Lengh = {input.Length}) = {ToBinary(input)}");

            uint a0 = 0x67452301;   // Buffer A
            uint b0 = 0xefcdab89;   // Buffer B
            uint c0 = 0x98badcfe;   // Buffer C
            uint d0 = 0x10325476;   // Buffer D

            //Chiều dài padding được chọn sao cho thông điệp cuối cùng có thể chia làm N block 512 bit 
            var addLength = (56 - ((input.Length + 1) % 64)) % 64; // calculate the new length with padding
            ListContent.Add("Lenght of Padding = " + addLength.ToString());
           
            var processedInput = new byte[input.Length + 1 + addLength + 8];
            ListContent.Add("Lenght of Process input = " + processedInput.Length.ToString());
            Array.Copy(input, processedInput, input.Length);
            processedInput[input.Length] = 0x80; // add 1

            byte[] length = BitConverter.GetBytes(input.Length * 8); // bit converter returns little-endian
            Array.Copy(length, 0, processedInput, processedInput.Length - 8, 4); // add length in bits

            ListContent.Add("Source array process input = " + ToBinary(length));
            ListContent.Add("process input = " + ToBinary(processedInput));
            
            for (int i = 0; i < processedInput.Length / 64; ++i)
            {
                // copy the input to M
                uint[] M = new uint[16];
                for (int j = 0; j < 16; ++j)
                    M[j] = BitConverter.ToUInt32(processedInput, (i * 64) + (j * 4));

                // initialize round variables
                uint A = a0, B = b0, C = c0, D = d0, F = 0, g = 0;

                // primary loop
                for (uint k = 0; k < 64; ++k)
                {
                    ListContent.Add($"Round {k}");

                    if (k <= 15)
                    {
                        F = (B & C) | (~B & D);
                        g = k;

                        ListContent.Add($"K <= 15 (g = {g}): F = (B & C) | (~B & D) = {F}");
                    }
                    else if (k >= 16 && k <= 31)
                    {
                        F = (D & B) | (~D & C);
                        g = ((5 * k) + 1) % 16;

                        ListContent.Add($"K >= 16 && K <= 31 (g = {g}): F = (D & B) | (~D & C) = {F}");
                    }
                    else if (k >= 32 && k <= 47)
                    {
                        F = B ^ C ^ D;
                        g = ((3 * k) + 5) % 16;

                        ListContent.Add($"K >= 32 && K <= 47 (g = {g}): F =  B ^ C ^ D = {F}");
                    }
                    else if (k >= 48)
                    {
                        F = C ^ (B | ~D);
                        g = (7 * k) % 16;

                        ListContent.Add($"K >= 48 (g = {g}): F = C ^ (B | ~D) = {F}");
                    }

                    var dtemp = D;
                    D = C;
                    C = B;
                    uint sum = (A + F + MD5.K[k] + M[g]);
                    B = B + MD5.leftRotate(sum, MD5.s[k]);
                    ListContent.Add($"(A + F + MD5.K[{k}] + M[{g}]) = ({A} + {F} + {MD5.K[k]} + {M[g]}) = {sum}");
                    ListContent.Add($"Left Rotate << {MD5.s[k]} = " + B);
                    A = dtemp;

                    ListContent.Add($"A{i} = {A}");
                    ListContent.Add($"B{i} = {B}");
                    ListContent.Add($"C{i} = {C}");
                    ListContent.Add($"D{i} = {D}");
                }

                a0 += A;
                b0 += B;
                c0 += C;
                d0 += D;

                ListContent.Add($"Block A[{i}] = {a0}");
                ListContent.Add($"Block B[{i}] = {b0}");
                ListContent.Add($"Block C[{i}] = {c0}");
                ListContent.Add($"Block D[{i}] = {d0}");
            }

            ListContent.Add("MD5 Hash = " + MD5.GetByteString(a0) + MD5.GetByteString(b0) + MD5.GetByteString(c0) + MD5.GetByteString(d0));
        }

        private void btnSocial_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string link = tbxLinkGithub.Text;
            
            if (btn.Tag.Equals("Github"))
                link = tbxLinkGithub.Text;
            else if (btn.Tag.Equals("Facebook"))
                link = tbxLinkFacebook.Text;
            else if (btn.Tag.Equals("Telegram"))
                link = tbxLinkTelegram.Text;
            else if (btn.Tag.Equals("Blog"))
                link = tbxLinkBlog.Text;

            DialogSocial d = new DialogSocial(btn.Tag.ToString());
            d.RedirectLink = link;
            HandyControl.Controls.Dialog.Show(d);
        }
    }
}
