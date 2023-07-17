using appTemplate.Logics;
using appTemplate.Views;
using HtmlAgilityPack;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using uPLibrary.Networking.M2Mqtt.Messages;
using MqttClient = uPLibrary.Networking.M2Mqtt.MqttClient;

namespace appTemplate
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public bool IsConnected { get; set; } // MQTT 접속 여부 확인하기 위함

        public DispatcherTimer timer; // 시간 실시간으로 받기 위해 타이머 설정

        public MainWindow()
        {
            InitializeComponent();
            
            // WindowState = WindowState.Maximized; // 실행시 전체화면
            WindowStartupLocation = WindowStartupLocation.CenterScreen; // 스크린 정 중앙에 창 띄우기 

            // Timer 생성 및 설정
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1초마다 업데이트
            timer.Tick += Timer_Tick;

            // Timer 시작
            timer.Start();
        }

        #region < 대시보드1 날씨영역 - 시간 실시간으로 받기 위한 메서드>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 날짜, 요일, 시간
            Txtdate.Text = DateTime.Today.ToShortDateString();
            Txtday.Text = DateTime.Now.DayOfWeek.ToString();
            TxtTime.Text = DateTime.Now.ToShortTimeString();
        }
        #endregion

        #region <메인 창 로드 영역 - 로그인 창 부분은 앱 구현 마지막 단계에 주석 지우고 사용!>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //    var loginWindow = new Login();
            //    loginWindow.Owner = this; // LoginWindow의 부모는 MainWindow
            //    loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // MainWindow의 정중앙에 위치
            //    loginWindow.ShowDialog();  // 모달창

            try
            {
                #region < MQTT >
                Commons.MQTT_CLIENT = new MqttClient(Commons.BROKERHOST); // MQTT 클라이언트 초기화
                Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived; // MQTT 메시지 수신 이벤트 핸들러 등록

                try
                {
                    if (!Commons.MQTT_CLIENT.IsConnected)
                    {
                        // MQTT 브로커에 연결
                        Commons.MQTT_CLIENT.Connect("MONITOR");
                        TxtLog.Text = ">>> MQTT Broker Connected";

                        // LED 상태를 확인하기 위해 구독
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.MQTTTOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                    }
                }
                catch (Exception ex)
                {
                    TxtLog.Text = $"MQTT Error: {ex.Message}";
                }
                #endregion

                #region < OpenAPI로 날씨값 받아오기 >
                //try
                //{
                //    await CheckWeatehr();
                //}
                //catch
                //{
                //    await Logics.Commons.ShowMessageAsync("오류", $"오류 발생 : 날씨 정보를 받아올 수 없습니다.");
                //    // 기상청 API 초단기실황 자체가 생성, 조회되는 기준시간이 있기 때문에 시간이 안맞으면 오류 발생 할 수 있음
                //    // 1. 24시간 동안만의 결과값을 제공  그 이전 값은 조회 오류 => 현재 날짜로 조회하기 때문에 이 문제는 해당X
                //    // 2. 기준시간 00시의 값은 00시 30분에 생성되어 제공되기 때문에 새벽 12시~12시 30분 사이에 조회하면 값이 없음 =>
                //}
                #endregion

                #region < 날씨 - 기상청 홈페이지 크롤링해서 받아오기>
                // 크롤링할 웹 사이트 URL 설정
                string url = "https://www.weather.go.kr/w/obs-climate/land/city-obs.do";

                // 웹 페이지 다운로드
                string htmlContent = DownloadWebPage(url);

                // HTML 파싱하여 부산 날씨 추출
                ParseWeatherData(htmlContent, "부산");

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show($"예외 발생: {ex.Message}\n스택 트레이스: {ex.StackTrace}", "예외", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
        }
        #endregion

        private string DownloadWebPage(string url)
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

        private void ParseWeatherData(string htmlContent, string city)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // table class="table-col" 테이블의 모든 행을 가져옴
            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//table[@class='table-col']//tr");

            if (rows != null)
            {
                bool foundCity = false;
                foreach (HtmlNode row in rows)
                {
                    // 행에서 모든 셀을 가져옴
                    HtmlNodeCollection cells = row.SelectNodes("td");

                    if (cells != null && cells.Count >= 7)
                    {
                        string location = cells[0].InnerText.Trim();
                        string cloud = cells[3].InnerText.Trim();
                      
                        string temperature = cells[5].InnerText.Trim();
                        string rainy = cells[8].InnerText.Trim();
                  
                        string humidity = cells[9].InnerText.Trim();
                        string wind = cells[11].InnerText.Trim();
         

                        // 지역 이름이 부산이면 데이터를 출력하고 반복을 종료함
                        if (location.Contains(city))
                        {
                            //GetWeatherImagePath(cloud2, rainy2);

                            TxtTemp.Text = $"{temperature} ℃";
                            
                            TxtHumid.Text = $"{humidity} %";
                            
                            TxtWind.Text = $"{wind} m/s";
                            //if (wind2 < 4) { Txtalarm.Text = "약함"; }
                            //else if (wind2 >= 4 && wind2 < 9) { Txtalarm.Text = "약간 강함"; }
                            //else if (wind2 >= 9 && wind2 < 14)
                            //{
                            //    Txtalarm.Foreground = Brushes.DarkOrange;
                            //    Txtalarm.Text = "강함";
                            //    foundCity = true;
                            //}
                            break;
                        }
                    }
                }

                if (!foundCity)
                {
                    Console.WriteLine($"'{city}'의 데이터를 찾을 수 없습니다.");
                }
            }
            else
            {
                Console.WriteLine("데이터 테이블을 찾을 수 없습니다.");
            }
        }

        
        

        #region < 차량 관리 버튼 이벤트 영역 - 자식창 띄우기>
        private void BtnMngCar_Click(object sender, RoutedEventArgs e)
        {
            var mngCarWindow = new MngCar();
            mngCarWindow.Owner = this;
            mngCarWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner; // 부모창 정중앙에 띄우기
            mngCarWindow.ShowDialog(); // 모달창
        }
        #endregion

        #region < 실제 OpenAPI 불러오는 함수 >
        //public async Task CheckWeatehr() 
        //{
        //    // openAPI 요청 uri
        //    string openApiUri = $"http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst?serviceKey={Commons.apiKey}&numOfRows=10&dataType=JSON&pageNo=1&base_date={Commons.convertedToday}&base_time={Commons.formattedTime}&nx=98&ny=74";
        //    // 하루 동안만의 결과값 제공함!
        //    string result = string.Empty; //결과값 초기화

        //    // API 실행할 WebRequest, WebResponse 객체
        //    WebRequest req = null;
        //    WebResponse res = null;
        //    StreamReader reader = null;

        //    try // API 요청
        //    {
        //        req = WebRequest.Create(openApiUri); // URL을 넣어서 객체를 생성
        //        res = await req.GetResponseAsync(); // 요청한 결과를 응답에 할당
        //        reader = new StreamReader(res.GetResponseStream());
        //        result = reader.ReadToEnd(); // json결과 텍스트로 저장
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex; //  MetroWindow_Loaded에서 오류메세지 보여주기 때문에 여기선 그냥 던져주기
        //    }
        //    finally
        //    {
        //        reader.Close();
        //        res.Close();
        //    }

        //    //result를 json으로 변경
        //    var jsonResult = JObject.Parse(result); // string -> json
        //    ///Debug.WriteLine(jsonResult.ToString());
        //    var item = jsonResult["response"]["body"]["items"]["item"]; // json 객체 key값인 item 으로 접근
        //    //Debug.WriteLine(item.ToString());
        //    var json_array = item as JArray;
        //    Debug.WriteLine(json_array.ToString());
        //    Debug.WriteLine(json_array.Type);

        //    var weatherResult = new List<Weather>(); // json에서 넘어온 배열 담을 리스트
        //    foreach (var val in json_array)
        //    {
        //        var WeatherResult = new Weather()
        //        {
        //            BaseDate = Convert.ToInt32(val["baseDate"]),
        //            BaseTime = Convert.ToInt32(val["baseTime"]),
        //            Category = Convert.ToString(val["category"]),
        //            NX = Convert.ToInt32(val["nx"]),
        //            NY = Convert.ToInt32(val["ny"]),
        //            ObsrValue = Convert.ToDouble(val["obsrValue"])
        //        };

        //        weatherResult.Add(WeatherResult);
        //    }

        //    /* category로 접근 - obsrValue 값 띄워야함.. category 중 실제로 사용할 것
        //     PTY(강수형태) : 없음(0), 비(1), 비/눈(2), 눈(3), 빗방울(5) -- 흐림으로 정의 , 빗방울눈날림(6) -- 흐림으로 정의, 눈날림(7) -- 흐림으로 정의
        //     REH(습도) : 실수값으로 반환
        //     T1H(기온) : 실수값으로 반환
        //     WSD(풍속) : 실수값으로 반환 / 4미만 : 바람약함 / 4 ~ 8 : 약간강(안면감촉, 나뭇잎 조금 흔들림) / 9 ~ 13 : 강(나무가지 깃발 가볍게 흔들림) / 14~ : 매우강(먼지 일고 나무 전체 흔들림) 
        //     */

        //    foreach (var weather in weatherResult)
        //    {
        //        switch (weather.Category) // Category 기준으로 화면에 값 띄워줌
        //        {
        //            case "PTY": // 날씨 이미지
        //                GetWeatherImagePath(weather.ObsrValue, weather);
        //                break;

        //            case "T1H": // 기온
        //                TxtTemp.Text = $"{weather.ObsrValue} ℃";
        //                break;

        //            case "REH": // 습도
        //                TxtHumid.Text = $"{weather.ObsrValue} %";
        //                break;

        //            case "WSD": // 풍속
        //                TxtWind.Text = $"{weather.ObsrValue} m/s";
        //                if (weather.ObsrValue < 4) { Txtalarm.Text = "약함"; }
        //                else if (weather.ObsrValue >= 4 && weather.ObsrValue < 9) { Txtalarm.Text = "약간 강함"; }
        //                else if (weather.ObsrValue >= 9 && weather.ObsrValue < 14) {
        //                    Txtalarm.Foreground = Brushes.DarkOrange;
        //                    Txtalarm.Text = "강함"; 
        //                }
        //                else {
        //                    Txtalarm.Foreground = Brushes.DarkRed;
        //                    Txtalarm.Text = "매우 강함"; 
        //                }
        //                break;
        //        }
        //    }
        //}
        #endregion
        private void GetWeatherImagePath(double cloud, double rainy)
        {
          
                if (cloud == 0 && rainy == 0)
                {
                    ImgWeather.Source = new BitmapImage(new Uri("/Resources/sunny.png", UriKind.Relative));
                }
                else if (cloud > 0 && rainy == 0)
                {
                    ImgWeather.Source = new BitmapImage(new Uri("/Resources/cloud.png", UriKind.Relative));
                }
                else if (cloud > 0 && rainy > 0)
                {
                    ImgWeather.Source = new BitmapImage(new Uri("/Resources/rainy.png", UriKind.Relative));
                }           
        }


        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;

            if (Commons.MQTT_CLIENT.IsConnected)
            {
                if (toggleSwitch.IsOn)
                {
                    // LED 켜기 메시지 발행
                    Commons.MQTT_CLIENT.Publish(Commons.MQTTTOPIC, Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                }
                else
                {
                    // LED 끄기 메시지 발행
                    Commons.MQTT_CLIENT.Publish(Commons.MQTTTOPIC, Encoding.UTF8.GetBytes("0"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                }
            }
        }

        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message);
            Debug.WriteLine(msg);

        }
    }
}
