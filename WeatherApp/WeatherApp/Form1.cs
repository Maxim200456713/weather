using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string APIKey = "50db84300e18453fad436666dd8e23c1"; /* это ключ */

        private void btn_search_Click(object sender, EventArgs e) /* функция которая срабатывает по нажанию кнопки поиск */
        {
            getWeather(); /* отправляет запрос и выводит погоду */
        }

        private void getWeather() /* функция получения погоды */
        {
            using (WebClient web = new WebClient()) /* создается клиент (запрос) */
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric", TbCity.Text, APIKey); /* создается строка по которой будет делаться запрос */
                var json = web.DownloadString(url); /* получение ответа */
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json); /* превращаю данные из json в weatherinfo */
                pic_icon.ImageLocation = "http://openweathermap.org/img/wn/" + Info.weather[0].icon + "@2x.png";
                lab_condtion.Text = Info.weather[0].main; /*статус погоды */
                lab_detail.Text = Info.weather[0].description; 
                lab_sunset.Text = convertDateTime(Info.sys.sunset).ToShortTimeString();
                lab_sunrise.Text = convertDateTime(Info.sys.sunrise).ToShortTimeString();
                lab_windspeed.Text = Info.wind.speed.ToString();
                lab_pressure.Text = Info.main.pressure.ToString();
                lab_temp.Text = Math.Ceiling(Info.main.temp).ToString();
            }
        }
        DateTime convertDateTime(long millisec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            day = day.AddSeconds(millisec).ToLocalTime();
            return day;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TbCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                getWeather();
        }
    }
}
