using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TwoDimensionalCode.TwoDimenCode;

namespace TwoDimensionalCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Bitmap SaveMap = null;
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void create2DC_Click(object sender, RoutedEventArgs e)
        {
            string tbStr = tbVal.Text;
            if (string.IsNullOrEmpty(tbStr))
            {
                MessageBox.Show("文本不能为空！");
            }
            else
            {
                Bitmap bitmap = TDC.create2DC(tbStr);
                // string filename = @"C:\Users\hsc\Desktop\TDC\"+DateTime.Now.ToString("yyyyMMddHHmmss") +".png";
                // bitmap.Save(filename, ImageFormat.Png);
                SaveMap = bitmap;
                tdcImg.Source = TDC.BitmapToBitmapImage(bitmap);
                //bitmap.Dispose();
            }


        }
        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void create1DC_Click(object sender, RoutedEventArgs e)
        {
            string tbStr = tbVal.Text;
            if (!string.IsNullOrEmpty(tbStr))
            {
                Regex reg = new Regex("^[0-9]+$");
                if (reg.IsMatch(tbStr))
                {
                    int flag = tbStr.Length % 2;
                    int a = flag;
                    if (flag == 0)
                    {
                        Bitmap bitmap = TDC.create1DC(tbStr);
                        SaveMap = bitmap;
                        tdcImg.Source = TDC.BitmapToBitmapImage(bitmap);
                    }
                    else
                    {
                        MessageBox.Show("数字个数不为偶数");
                        tbVal.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("生成条形码的原始数据必须全为数字");
                    tbVal.Text = "";
                }

                //bitmap.Dispose();
            }
            else
            {
                MessageBox.Show("文本为空无法生成！");
               
            }

        }
        /// <summary>
        /// 解析二维码 图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decode2DC_Click(object sender, RoutedEventArgs e)
        {
            string fname= filename.Text;

            if (!string.IsNullOrEmpty(fname))
            {
                Bitmap bitmap = new Bitmap(fname);
                show2DC.Source = TDC.BitmapToBitmapImage(bitmap);

                tbMulti.Text = TDC.decode2DC(fname);
            }
            else
            {
                MessageBox.Show("还未选取要解析的文件");
            }
        }
        /// <summary>
        /// 解析显示选取的二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filename_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
                try
                {
                    OpenFileDialog openFile = new OpenFileDialog();

                    openFile.Filter = "文本文件|*.png";
                    openFile.FilterIndex = 2;
                    openFile.RestoreDirectory = true;
                    // openFile.ShowDialog();

                    if (openFile.ShowDialog() == true)
                    {
                        filename.Text = openFile.FileName;
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("异常提醒：" + ex.ToString());
                }
           
            
        }
        private void LogoName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
                OpenFileDialog openFile = new OpenFileDialog();
            //"图片文件(*.jpg, *.png, *.ico, *.bmp, *.gif) | *.jgp; *.png; *.ico; *.bmp; *.gif | All files(*.*) | *.* ";
            // openFile.Filter = "图片文件(*.jpg, *.png, *.ico, *.bmp, *.gif) | *.jgp; *.png; *.ico; *.bmp; *.gif";// | All files(*.*) | *.* ";  //"文本文件 |*.jpg;*.png;*.ico"; //"xls 文件（*.xls;*.xlsx)|*.xls;*.xlsx";
            openFile.Filter = "文本文件 |*.ico";
            openFile.FilterIndex = 4;
                openFile.RestoreDirectory = true;
                // openFile.ShowDialog();


                if (openFile.ShowDialog() == true)
                {
                    LogoName.Text = openFile.FileName;
                }
            
            
        }

        #region 创建带logo的二维码
        private void createLogo2DC_Click(object sender, RoutedEventArgs e)
        {
            string logoName = LogoName.Text;
            string strVal = strText.Text;
            if (string.IsNullOrEmpty(logoName) || string.IsNullOrEmpty(strVal))
            {
                MessageBox.Show("请确保logo文件路径和文本框内容不为空！");
            }
            else
            {
                Bitmap bitmap = TDC.create2DCWithLogo(logoName, strVal);
                SaveMap = bitmap;
                LogoTdcImg.Source = TDC.BitmapToBitmapImage(bitmap);
            }
            
        }

        /// <summary>
        /// 为啥 要用preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogoName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "文本文件|*.jpg;*.png;*.ico";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            // openFile.ShowDialog();


            if (openFile.ShowDialog() == true)
            {
                LogoName.Text = openFile.FileName;
            }

        }

        #endregion

        /// <summary>
        /// 保存图片  通用方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDC_Click(object sender, RoutedEventArgs e)
        {
            if (SaveMap != null)
            {

           
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "文本文件|*.png";
            fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            if (fileDialog.ShowDialog().Value == true)
            {
                string fileName = fileDialog.FileName;
                SaveMap.Save(fileName, ImageFormat.Png);
            }
            }
            else
            {
                MessageBox.Show("还未生成图片不能保存操作");
            }
        }

        /// <summary>
        /// 清空图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearDCBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.LogoTdcImg.Source = null;
            if(sender.Equals(Clear2DCBtn))
            {
                tdcImg.Source = null;
            }
            else
            {
                LogoTdcImg.Source = null;
            }
           // UIElement element = new UIElement();
            //(element as System.Windows.Controls.Image).Source = null;

            //foreach (UIElement element in TDCGrid.Children)
            //{
            //    if (element is TextBox)
            //    {
            //        (element as TextBox).Text = "";
            //    }
            //    if(element is System.Windows.Controls.Image)
            //    {
            //        (element as System.Windows.Controls.Image).Source = null;
            //    }

            //}
        }
    }
}
