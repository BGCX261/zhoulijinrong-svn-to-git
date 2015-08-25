using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tstEditOnline
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string doclist = @"<?xml version='1.0' encoding='utf-8'?>
<附件>
<item><Type>bmp</Type><Alias>123</Alias><Title>123.bmp</Title><Encode></Encode><Size>750.05K</Size><Edition></Edition><URL>200908/200908171432302810.bmp</URL><fullURL>http://172.29.128.239/Temp/200908/200908171432302810.bmp</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200908</FolderName><FileName>200908171432302810.bmp</FileName><IsCopy>False</IsCopy><WorkItemID>1123</WorkItemID><iPage></iPage></item>
<item><Type>docx</Type><Alias>1</Alias><Title>200909011401557657.docx</Title><Encode></Encode><Size>13.24K</Size><Edition></Edition><URL>200909/200909011401557657.docx</URL><fullURL>http://172.29.128.239/Temp/200909/200909011401557657.docx</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909011401557657.docx</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
</附件>";
            /*
<item><Type>docx</Type><Alias>Version5</Alias><Title>200909020856315465.docx</Title><Encode></Encode><Size>10.48K</Size><Edition></Edition><URL>200909/200909020856315465.docx</URL><fullURL>http://172.29.128.239/Temp/200909/200909020856315465.docx</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909020856315465.docx</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>KB935839</Alias><Title>2009090214050406.log</Title><Encode></Encode><Size>6.5K</Size><Edition></Edition><URL>200909/2009090214050406.log</URL><fullURL>http://172.29.128.239/Temp/200909/2009090214050406.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>2009090214050406.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>txt</Type><Alias>OEWABLog</Alias><Title>200909021405042967.txt</Title><Encode></Encode><Size>1.82K</Size><Edition></Edition><URL>200909/200909021405042967.txt</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405042967.txt</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405042967.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>pop3oc</Alias><Title>200909021405043755.log</Title><Encode></Encode><Size>5.13K</Size><Edition></Edition><URL>200909/200909021405043755.log</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405043755.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405043755.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>regopt</Alias><Title>200909021405044681.log</Title><Encode></Encode><Size>796字节</Size><Edition></Edition><URL>200909/200909021405044681.log</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405044681.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405044681.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>txt</Type><Alias>relect</Alias><Title>200909021405045310.txt</Title><Encode></Encode><Size>3.05K</Size><Edition></Edition><URL>200909/200909021405045310.txt</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405045310.txt</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405045310.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>sessmgr.setup</Alias><Title>200909021405046097.log</Title><Encode></Encode><Size>849字节</Size><Edition></Edition><URL>200909/200909021405046097.log</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405046097.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405046097.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>txt</Type><Alias>websrvaddress</Alias><Title>200909021405047186.txt</Title><Encode></Encode><Size>906字节</Size><Edition></Edition><URL>200909/200909021405047186.txt</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405047186.txt</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405047186.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>wmsetup10</Alias><Title>200909021405047960.log</Title><Encode></Encode><Size>379字节</Size><Edition></Edition><URL>200909/200909021405047960.log</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405047960.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405047960.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>wmsetup</Alias><Title>200909021405048593.log</Title><Encode></Encode><Size>5.39K</Size><Edition></Edition><URL>200909/200909021405048593.log</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405048593.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405048593.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>imsins</Alias><Title>200909021405049683.log</Title><Encode></Encode><Size>3.34K</Size><Edition></Edition><URL>200909/200909021405049683.log</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405049683.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405049683.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>log</Type><Alias>KB914783</Alias><Title>20090902140505465.log</Title><Encode></Encode><Size>630字节</Size><Edition></Edition><URL>200909/20090902140505465.log</URL><fullURL>http://172.29.128.239/Temp/200909/20090902140505465.log</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>20090902140505465.log</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>inf</Type><Alias>weboffice</Alias><Title>200909021405307038.inf</Title><Encode></Encode><Size>761字节</Size><Edition></Edition><URL>200909/200909021405307038.inf</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405307038.inf</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405307038.inf</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>txt</Type><Alias>relect</Alias><Title>200909021405307653.txt</Title><Encode></Encode><Size>3.05K</Size><Edition></Edition><URL>200909/200909021405307653.txt</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405307653.txt</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405307653.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>txt</Type><Alias>问题</Alias><Title>200909021405308282.txt</Title><Encode></Encode><Size>50字节</Size><Edition></Edition><URL>200909/200909021405308282.txt</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405308282.txt</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405308282.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>txt</Type><Alias>新建文本文档</Alias><Title>200909021405308906.txt</Title><Encode></Encode><Size>6.87K</Size><Edition></Edition><URL>200909/200909021405308906.txt</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405308906.txt</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405308906.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>inf</Type><Alias>Editor</Alias><Title>200909021405309372.inf</Title><Encode></Encode><Size>736字节</Size><Edition></Edition><URL>200909/200909021405309372.inf</URL><fullURL>http://172.29.128.239/Temp/200909/200909021405309372.inf</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909021405309372.inf</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
<item><Type>xml</Type><Alias>1%</Alias><Title>200909070930514370.xml</Title><Encode></Encode><Size>8.42K</Size><Edition></Edition><URL>200909/200909070930514370.xml</URL><fullURL>http://172.29.128.239/Temp/200909/200909070930514370.xml</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909070930514370.xml</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item>
</附件>";*/

            //string doclist = @"<?xml version='1.0' encoding='utf-8'?><附件><item><Type>docx</Type><Alias>Nontrack</Alias><Title>200909031125036560.docx</Title><Encode></Encode><Size>10.05K</Size><Edition></Edition><URL>200909/200909031125036560.docx</URL><fullURL>http://172.29.128.239/Temp/200909/200909031125036560.docx</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909031125036560.docx</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item><item><Type>docx</Type><Alias>Tracking</Alias><Title>200909031125044212.docx</Title><Encode></Encode><Size>10.07K</Size><Edition></Edition><URL>200909/200909031125044212.docx</URL><fullURL>http://172.29.128.239/Temp/200909/200909031125044212.docx</fullURL><ProcessType>Temp</ProcessType><IsZhengWen></IsZhengWen><FolderName>200909</FolderName><FileName>200909031125044212.docx</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item></附件>";
            axEditorOnline1.SetDocLibLst(doclist);
            axEditorOnline1.SetConfigInfo("http://172.29.128.239/MossService/DocumentLibrary.asmx,C:\\LocalTemp,http://172.29.128.239/QSOA,某某,");
            //0, 1, 2
            axEditorOnline1.SetEnableEditCol(1, false);
            //axEditorOnline1.AutoShowAxWnd("123.bmp");
            //axEditorOnline1.ShowAxWnd();

            string str = axEditorOnline1.GetDocLibLst();

            string ss = @"<?xml version='1.0' encoding='utf-8'?><附件><item><Type>txt</Type><Alias>新建 文本文档</Alias><Title>200911251615349845.txt</Title><Encode></Encode><Size>5.7K</Size><Edition></Edition><URL>200911/200911251615349845.txt</URL><fullURL>http://172.29.128.239/QSOA/DocLib/200911/200911251615349845.txt</fullURL><ProcessType>公司发文</ProcessType><IsZhengWen></IsZhengWen><FolderName>200911</FolderName><FileName>200911251615349845.txt</FileName><IsCopy>False</IsCopy><WorkItemID></WorkItemID><iPage></iPage></item></附件>";
        }
    }
}