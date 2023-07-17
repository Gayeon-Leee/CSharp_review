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
using System.Net;
using HtmlAgilityPack;
using System.Diagnostics;

namespace Webtest1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 크롤링할 웹 사이트 URL 설정
            string url = "https://www.weather.go.kr/w/obs-climate/land/city-obs.do";

            // 웹 페이지 다운로드
            string htmlContent = DownloadWebPage(url);

            // HTML 파싱
            ParseHtml(htmlContent);
        }

        static string DownloadWebPage(string url)
        {
            string htmlContent;
            using (WebClient client = new WebClient())
            {
                // UTF-8로 인코딩된 웹 페이지 다운로드
                client.Encoding = System.Text.Encoding.UTF8;
                htmlContent = client.DownloadString(url);
            }
            return htmlContent;
        }

        static void ParseHtml(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // 원하는 정보 추출을 위해 XPath를 사용하여 HTML 요소 선택
            // 예를 들어, 모든 링크를 가져오려면:
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes != null)
            {
                foreach (HtmlNode linkNode in linkNodes)
                {
                    string linkUrl = linkNode.GetAttributeValue("href", "");
                    Debug.Write(linkUrl);
                }
            }
            else
            {
                Debug.Write("링크가 없습니다.");
            }
        }
    }
}
