using OtpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPMLoginAPISampleSeleniumAndPuppeteer.Libs;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using PuppeteerSharp;
using static System.Net.WebRequestMethods;
using IOFile = System.IO.File;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Diagnostics;
using System.Net.NetworkInformation;
using OpenQA.Selenium.DevTools;
using IniParser;
using IniParser.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Text.RegularExpressions;
using System.Dynamic;

namespace Auto_Tool_AMZ_with_GPM
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private List<Profile> profiles = new List<Profile>();
        private string[] _listCard;
        private string[] _listAccount;
        private string[] _listURL;
        private string[] _listImage;
        private int windowCount = 0;
        private int _totalWindows = 0;
        private int _totalAccCheck = 0;
        private int _totalAccDie = 0;
        private int _totalAccPass = 0;
        private int _totalCardLive = 0;
        private int _totalCardDie = 0;
        private readonly string configPath = "config.ini";
        private FileIniDataParser parser;
        private IniData data;
        //---------------------------------------
        public Form1()
        {
            InitializeComponent();
            LoadNewLogListView();
            parser = new FileIniDataParser();
            EnsureConfigFile();
            LoadConfig();
        }
        private void LoadNewLogListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Chrome", 280);
            listView1.Columns.Add("Email", 240);
            listView1.Columns.Add("Time", 170);
            listView1.Columns.Add("Status", 70);
        }

        private void EnsureConfigFile()
        {
            if (!System.IO.File.Exists(configPath))
            {
                data = new IniData();
                data["Settings"]["Proxy"] = "";
                data["Settings"]["GPM_API"] = "";
                data["Settings"]["ChromeVersion"] = "";
                parser.WriteFile(configPath, data);
            }
        }

        private void LoadConfig()
        {
            data = parser.ReadFile(configPath);

            txtProxy.Text = data["Settings"]["Proxy"];
            txtGPMAPI.Text = data["Settings"]["GPM_API"];
            txtChromeVersion.Text = data["Settings"]["ChromeVersion"];

            if (string.IsNullOrWhiteSpace(txtGPMAPI.Text) ||
                string.IsNullOrWhiteSpace(txtChromeVersion.Text))
            {
                MessageBox.Show("Config trong file config.ini thiếu", "Config.ini MISSING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveConfig()
        {
            data["Settings"]["Proxy"] = txtProxy.Text;
            data["Settings"]["GPM_API"] = txtGPMAPI.Text;
            data["Settings"]["ChromeVersion"] = txtChromeVersion.Text;
            parser.WriteFile(configPath, data);
        }

        private void addItemToListView(string Chrome, string Email, string Status)
        {
            // Columns: Chrome, Email, Time, Status
            // Current data time
            string time = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            string[] row = { Chrome, Email, time, Status };
            var listViewItem = new ListViewItem(row);
            listView1.Items.Add(listViewItem);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
        }

        private void txtProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProxy.Checked)
            {
                txtProxy.ReadOnly = false;
            }
            else
            {
                txtProxy.ReadOnly = true;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            // Change text of button to STOP
            if (btnRun.Text == "STOP")
            {
                btnRun.Text = "START";
                btnRun.BackColor = Color.ForestGreen;
                timer1.Stop();
                txtAMZAccount.ReadOnly = false;
                txtListCard.ReadOnly = false;
                txtGPMAPI.ReadOnly = false;
                txtURL.ReadOnly = false;
                txtImageDie.ReadOnly = false;
                return;
            }

            if (!checkAllRequirements())
            {
                return;
            }
            this._totalAccCheck = 0;
            this._totalAccDie = 0;
            this._totalAccPass = 0;
            this._totalWindows = 0;
            this._totalCardLive = 0;
            this._totalCardDie = 0;
            txtAMZAccount.ReadOnly = true;
            txtListCard.ReadOnly = true;
            txtGPMAPI.ReadOnly = true;
            txtURL.ReadOnly = true;
            txtImageDie.ReadOnly = true;
            btnRun.Text = "STOP";
            btnRun.BackColor = Color.Red;

            timer1.Start();
        }

        public bool IsConnectedToInternet()
        {
            string host = "1.1.1.1";
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return result;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (this._totalWindows < nudChrome.Value)
            {
                var tasks = new List<Task>();
                try
                {
                    // if checkAllRequirements(), going to catch
                    if (!checkAllRequirements())
                    {
                        throw new Exception();
                    }

                    int waited = 0;
                    while (!IsConnectedToInternet() && waited < 60)
                    {
                        await Task.Delay(1000);
                        waited++;
                    }
                    if (!IsConnectedToInternet())
                    {
                        MessageBox.Show("Không có kết nối Internet sau 1 phút chờ. Vui lòng kiểm tra kết nối của bạn.", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    this._listURL = IOFile.ReadAllLines(txtURL.Text);

                    string[] acc = this._listAccount[0].Split('|');
                    windowCount++;
                    tasks.Add(startOneGPM(acc[0], acc[1], acc[2], windowCount));
                    if (windowCount == nudChrome.Value)
                    {
                        windowCount = 0;
                    }
                    _totalWindows++;
                    lblCardLive.Text = "Card LIVE: " + _totalCardLive.ToString();
                    lblCardDie.Text = "DIE: " + _totalCardDie.ToString();
                    lblAccDie.Text = "Acc DIE: " + _totalAccDie.ToString();
                    lblAccPass.Text = "Acc PASS: " + _totalAccPass.ToString();

                    this._listAccount = this._listAccount.Skip(1).ToArray();
                    lblAccount.Text = this._listAccount.Length.ToString();
                    IOFile.WriteAllLines(txtAMZAccount.Text, this._listAccount);
                }
                catch (Exception ex)
                {
                    timer1.Stop();
                    btnRun.Text = "START";
                    btnRun.BackColor = Color.ForestGreen;
                    txtAMZAccount.ReadOnly = false;
                    txtListCard.ReadOnly = false;
                    txtGPMAPI.ReadOnly = false;
                    txtURL.ReadOnly = false;
                    txtImageDie.ReadOnly = false;
                    MessageBox.Show("Dữ liệu sai định dạng hoặc đã hết.\nHãy kiểm tra lại dữ liệu input!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                await Task.WhenAll(tasks);
            }
        }

        private string GenerateRandomBusinessName()
        {
            string[] businessNames = {"Tech Solutions Inc.", "Blue Ocean Ventures", "Pinnacle Consulting Group", "GreenLeaf Marketing", "Silverline Logistics","Urban Edge Realty", "NextGen Innovations", "Summit Financial Partners", "BrightPath Education", "Redwood Construction","Sunrise Media Group", "Evergreen Landscaping", "Velocity IT Services", "Crystal Clear Cleaning", "Peak Performance Fitness","Golden Gate Advisors", "Skyline Travel Agency", "Maplewood Publishing", "Quantum Engineering", "Starlight Events","Crestview Healthcare", "Lighthouse Insurance", "Aspire Digital Agency", "Ironclad Security", "Harmony Wellness Center","Blue Ridge Apparel", "Momentum Motors", "Clever Fox Design", "NorthStar Consulting", "FreshStart Foods","Elite Staffing Solutions", "Silver Oak Winery", "Urban Roots Cafe", "Bright Future Tutoring", "Precision Auto Repair","Sunset Photography", "Everest Outdoor Gear", "GreenTech Energy", "Crystal Waters Spa", "Summit Legal Group","Red Maple Bistro", "Blue Sky Aviation", "Peak Performance Coaching", "Golden State Marketing", "Skyline Construction","Maple Leaf Accounting", "Quantum Data Systems", "Starlight Entertainment", "Crestview Dental", "Lighthouse Realty","Aspire Fitness Studio", "Ironclad Manufacturing", "Harmony Music School", "Blue Ridge Consulting", "Momentum Marketing","Clever Fox Media", "NorthStar Financial", "FreshStart Cleaning", "Elite Event Planning", "Silver Oak Consulting","Urban Roots Market", "Bright Future Learning", "Precision Engineering", "Sunset Travel", "Everest Consulting","GreenTech Solutions", "Crystal Waters Cleaning", "Summit Accounting", "Red Maple Consulting", "Blue Sky Media","Peak Performance Training", "Golden State Realty", "Skyline Marketing", "Maple Leaf Consulting", "Quantum Innovations","Starlight Productions", "Crestview Consulting", "Lighthouse Marketing", "Aspire Consulting", "Ironclad Logistics","Harmony Consulting", "Blue Ridge Marketing", "Momentum Consulting", "Clever Fox Consulting", "NorthStar Marketing","FreshStart Consulting", "Elite Consulting Group", "Silver Oak Marketing", "Urban Roots Consulting", "Bright Future Consulting","Precision Consulting", "Sunset Consulting", "Everest Marketing", "GreenTech Consulting", "Crystal Waters Consulting","Summit Consulting", "Red Maple Marketing", "Blue Sky Consulting", "Peak Performance Consulting", "Golden State Consulting","Skyline Consulting", "Maple Leaf Marketing", "Quantum Consulting", "Starlight Consulting", "Crestview Marketing","Lighthouse Consulting", "Aspire Marketing", "Ironclad Consulting", "Harmony Marketing", "Blue Ridge Consulting Group","Momentum Marketing Group", "Clever Fox Marketing", "NorthStar Consulting Group", "FreshStart Marketing", "Elite Marketing Group","Silver Oak Consulting Group", "Urban Roots Marketing", "Bright Future Marketing", "Precision Marketing", "Sunset Marketing","Everest Consulting Group", "GreenTech Marketing", "Crystal Waters Marketing", "Summit Marketing", "Red Maple Consulting Group","Blue Sky Marketing", "Peak Performance Marketing", "Golden State Marketing Group", "Skyline Marketing Group", "Maple Leaf Consulting Group","Quantum Marketing", "Starlight Marketing", "Crestview Consulting Group", "Lighthouse Marketing Group", "Aspire Consulting Group","Ironclad Marketing", "Harmony Consulting Group", "Blue Ridge Marketing Group", "Momentum Consulting Group", "Clever Fox Consulting Group","NorthStar Marketing Group", "FreshStart Consulting Group", "Elite Consulting", "Silver Oak Marketing Group", "Urban Roots Consulting Group","Bright Future Consulting Group", "Precision Consulting Group", "Sunset Consulting Group", "Everest Marketing Group", "GreenTech Consulting Group","Crystal Waters Consulting Group", "Summit Consulting Group", "Red Maple Marketing Group", "Blue Sky Consulting Group", "Peak Performance Consulting Group","Golden State Consulting Group", "Skyline Consulting Group", "Maple Leaf Marketing Group", "Quantum Consulting Group", "Starlight Consulting Group","Crestview Marketing Group", "Lighthouse Consulting Group", "Aspire Marketing Group", "Ironclad Consulting Group", "Harmony Marketing Group","Blue Ridge Consulting", "Momentum Consulting", "Clever Fox Consulting", "NorthStar Consulting", "FreshStart Consulting", "Elite Consulting","Silver Oak Consulting", "Urban Roots Consulting", "Bright Future Consulting", "Precision Consulting", "Sunset Consulting", "Everest Consulting","GreenTech Consulting", "Crystal Waters Consulting", "Summit Consulting", "Red Maple Consulting", "Blue Sky Consulting", "Peak Performance Consulting","Golden State Consulting", "Skyline Consulting", "Maple Leaf Consulting", "Quantum Consulting", "Starlight Consulting", "Crestview Consulting","Lighthouse Consulting", "Aspire Consulting", "Ironclad Consulting", "Harmony Consulting", "Blue Ridge Consulting Group", "Momentum Consulting Group","Clever Fox Consulting Group", "NorthStar Consulting Group", "FreshStart Consulting Group", "Elite Consulting Group", "Silver Oak Consulting Group","Urban Roots Consulting Group", "Bright Future Consulting Group", "Precision Consulting Group", "Sunset Consulting Group", "Everest Consulting Group","GreenTech Consulting Group", "Crystal Waters Consulting Group", "Summit Consulting Group", "Red Maple Consulting Group", "Blue Sky Consulting Group","Peak Performance Consulting Group", "Golden State Consulting Group", "Skyline Consulting Group", "Maple Leaf Consulting Group", "Quantum Consulting Group"
            };

            Random random = new Random();
            int index = random.Next(businessNames.Length);
            return businessNames[index];
        }

        private string GenerateRandomUSFullName()
        {
            Random random = new Random();
            string[] firstNames = {
                "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Jessica", "James", "Ashley",
                "William", "Linda", "Richard", "Barbara", "Joseph", "Susan", "Thomas", "Margaret", "Charles", "Lisa",
                "Christopher", "Karen", "Daniel", "Nancy", "Matthew", "Betty", "Anthony", "Sandra", "Mark", "Donna",
                "Paul", "Carol", "Steven", "Ruth", "Andrew", "Sharon", "Kenneth", "Michelle", "Joshua", "Laura",
                "Kevin", "Sarah", "Brian", "Kimberly", "George", "Deborah", "Edward", "Jessica", "Ronald", "Shirley",
                "Frank", "Amy", "Scott", "Angela", "Eric", "Melissa", "Stephen", "Brenda", "Larry", "Cynthia",
                "Jeffrey", "Kathleen", "Ryan", "Pamela", "Jacob", "Martha", "Gary", "Rebecca", "Nicholas", "Dorothy",
                "Jonathan", "Evelyn", "Justin", "Jean", "Brandon", "Cheryl", "Samuel", "Mildred", "Benjamin", "Katherine",
                "Gregory", "Joan", "Alexander", "Ashley", "Patrick", "Judith", "Jack", "Rose", "Dennis", "Janet",
                "Jerry", "Maria", "Tyler", "Heather", "Aaron", "Diane", "Henry", "Julie", "Douglas", "Joyce",
                "Peter", "Victoria", "Adam", "Kelly", "Zachary", "Christina", "Nathan", "Lauren", "Walter", "Frances",
                "Harold", "Megan", "Kyle", "Ann", "Carl", "Alice", "Arthur", "Jacqueline", "Gerald", "Hannah",
                "Roger", "Olivia", "Keith", "Gloria", "Jeremy", "Megan", "Lawrence", "Teresa", "Terry", "Sara",
                "Sean", "Janice", "Christian", "Doris", "Albert", "Julia", "Joe", "Grace", "Ethan", "Judy",
                "Austin", "Theresa", "Jesse", "Beverly", "Willie", "Denise", "Billy", "Marilyn", "Bryan", "Amber",
                "Bruce", "Madison", "Jordan", "Danielle", "Ralph", "Brittany", "Roy", "Diana", "Noah", "Abigail",
                "Dylan", "Jane", "Eugene", "Lori", "Wayne", "Rachel", "Alan", "Andrea", "Juan", "Catherine",
                "Louis", "Kayla", "Russell", "Charlotte", "Gabriel", "Victoria", "Randy", "Mia", "Philip", "Aubrey",
                "Harry", "Avery", "Vincent", "Brooklyn", "Bobby", "Savannah", "Johnny", "Addison", "Logan", "Eleanor"
            };

            string[] lastNames = {
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
                "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
                "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
                "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts",
                "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", "Cruz", "Edwards", "Collins", "Reyes",
                "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper",
                "Peterson", "Bailey", "Reed", "Kelly", "Howard", "Ramos", "Kim", "Cox", "Ward", "Richardson",
                "Watson", "Brooks", "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza", "Ruiz", "Hughes",
                "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers", "Long", "Ross", "Foster", "Jimenez",
                "Powell", "Jenkins", "Perry", "Russell", "Sullivan", "Bell", "Coleman", "Butler", "Henderson", "Barnes",
                "Gonzales", "Fisher", "Vasquez", "Simmons", "Romero", "Jordan", "Patterson", "Alexander", "Hamilton", "Graham",
                "Reynolds", "Griffin", "Wallace", "Moreno", "West", "Cole", "Hayes", "Bryant", "Herrera", "Gibson",
                "Ellis", "Tran", "Medina", "Aguilar", "Stevens", "Murray", "Ford", "Castro", "Marshall", "Owens",
                "Harrison", "Fernandez", "McDonald", "Woods", "Washington", "Kennedy", "Wells", "Vargas", "Henry", "Chen",
                "Freeman", "Webb", "Tucker", "Guzman", "Burns", "Crawford", "Olson", "Simpson", "Porter", "Hunter",
                "Gordon", "Mendez", "Silva", "Shaw", "Snyder", "Mason", "Dixon", "Munoz", "Hunt", "Hicks",
                "Holmes", "Palmer", "Wagner", "Black", "Robertson", "Boyd", "Rose", "Stone", "Salazar", "Fox",
                "Warren", "Mills", "Meyer", "Rice", "Schmidt", "Garza", "Daniels", "Ferguson", "Nichols", "Stephens",
                "Soto", "Weaver", "Ryan", "Gardner", "Payne", "Grant", "Dunn", "Kelley", "Spencer", "Hawkins"
            };

            string firstName = firstNames[random.Next(firstNames.Length)];
            string lastName = lastNames[random.Next(lastNames.Length)];

            return $"{firstName} {lastName}";
        }

        private async Task getAllProfiles()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    this.profiles = new List<Profile>();
                    var response = await httpClient.GetStringAsync(txtGPMAPI.Text + "/api/v3/profiles");
                    var jsonResponse = JObject.Parse(response);
                    var profiles = jsonResponse["data"].ToArray();
                    foreach (var profile in profiles)
                    {
                        Profile p = JsonConvert.DeserializeObject<Profile>(profile.ToString());
                        this.profiles.Add(p);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể lấy dữ liệu từ GPM-Login\nHãy mở GPM-Login lên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string createNewProfile(string profileName = "")
        {
            string profileId = "";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string[] arrOS = {
                        "Windows 11",
                        "Windows 10",
                        "Windows 8.1",
                        "Windows 8",
                        "Windows 7",
                        "Windows Server 2012",
                        "Windows Server 2012 R2",
                        "Windows Server 2012 Standard",
                        "Windows Server 2016",
                        "Windows Server 2016 Standard",
                        "Windows Server 2019",
                        "Windows Server 2019 Standard",
                    };
                    var postData = new StringContent(JsonConvert.SerializeObject(new
                    {
                        profile_name = profileName,
                        group_name = "All",
                        browser_core = "chromium",
                        browser_name = "Chrome",
                        is_random_browser_version = false,
                        browser_version = string.IsNullOrEmpty(txtChromeVersion.Text) ? "" : txtChromeVersion.Text,
                        raw_proxy = cbProxy.Checked ? txtProxy.Text : "",
                        startup_urls = "",
                        is_masked_font = true,
                        is_noise_canvas = false,
                        is_noise_webgl = false,
                        is_noise_client_rect = false,
                        is_noise_audio_context = true,
                        is_random_screen = true,
                        is_masked_webgl_data = true,
                        is_masked_media_device = true,
                        is_random_os = false,
                        user_agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36",
                        os = arrOS[new Random().Next(arrOS.Length)],
                    }), Encoding.UTF8, "application/json");
                    // POST /api/v3/profiles/create
                    var response = httpClient.PostAsync(txtGPMAPI.Text + "/api/v3/profiles/create", postData).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var jsonResponse = JObject.Parse(responseContent);
                    profileId = Convert.ToString(jsonResponse["data"]["id"]);
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    timer1.Stop();
                    btnRun.Text = "START";
                    btnRun.BackColor = Color.ForestGreen;
                    txtAMZAccount.ReadOnly = false;
                    txtListCard.ReadOnly = false;
                    txtGPMAPI.ReadOnly = false;
                    txtURL.ReadOnly = false;
                    txtImageDie.ReadOnly = false;
                    MessageBox.Show("Không thể tạo Profile.\nHãy mở GPM-Login lên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return profileId;
        }
        private bool closeProfile(string id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.DeleteAsync(txtGPMAPI.Text + "/api/v3/profiles/close/" + id).Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool deleteProfile(string id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.DeleteAsync(txtGPMAPI.Text + "/api/v3/profiles/delete/" + id + "?mode=2").Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task PerformActionWithWait(IPage page, string selector, Func<Task> action, bool isTypingAction = false, int timeout = 10000)
        {
            try
            {
                await page.WaitForSelectorAsync(selector, new WaitForSelectorOptions { Timeout = timeout });

                if (isTypingAction)
                {
                    await page.EvaluateExpressionAsync($"document.querySelector('{selector}').value = ''");
                }

                await action();
                await Task.Delay(new Random().Next(1000, 2000));

            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private async Task SelectActionInIframe(IPage page, string iframeSelector, string elementSelector, string selectedItem)
        {
            try
            {
                var element = await GetElementInIframe(page, iframeSelector, elementSelector);

                await element.SelectAsync(selectedItem);

                await Task.Delay(new Random().Next(1000, 2000));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task ClickActionInIframe(IPage page, string iframeSelector, string elementSelector)
        {
            try
            {
                var element = await GetElementInIframe(page, iframeSelector, elementSelector);

                await element.ClickAsync();

                await Task.Delay(new Random().Next(1000, 2000));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        private async Task TypeActionInIframe(IPage page, string iframeSelector, string elementSelector, string message, string email = "")
        {
            try
            {
                var element = await GetElementInIframe(page, iframeSelector, elementSelector);

                await element.TypeAsync(message);

                await Task.Delay(new Random().Next(1000, 2000));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        private async Task<IElementHandle> GetElementInIframe(IPage page, string iframeSelector, string elementSelector)
        {
            // Get the iframe element
            var iframeElement = await page.QuerySelectorAsync(iframeSelector);
            if (iframeElement == null)
            {
                throw new Exception($"Iframe with selector '{iframeSelector}' not found.");
            }

            // Get the iframe content frame
            var iframe = await iframeElement.ContentFrameAsync();
            if (iframe == null)
            {
                throw new Exception("Unable to get iframe content frame.");
            }

            // Select the desired element within the iframe
            var element = await iframe.QuerySelectorAsync(elementSelector);
            if (element == null)
            {
                throw new Exception($"Element with selector '{elementSelector}' not found in iframe.");
            }

            return element;
        }

        public async Task ClickButtonByXPath(IPage page, string xPath)
        {
            // Wait for the element to be present in the DOM
            await page.WaitForXPathAsync(xPath);

            // Find the element by XPath
            var elements = await page.XPathAsync(xPath);

            // Ensure the element was found
            if (elements.Length > 0)
            {
                // Click the first element found
                await elements[0].ClickAsync();
            }
            else
            {
                throw new Exception("Element not found");
            }
        }


        private async Task startOneGPM(string email, string password, string code2FA, int windows)
        {
            IOFile.AppendAllText("./_output/" + DateTime.Now.ToString("yyyy-MM-dd") + "-log.txt", DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + " - " + email + " - " + password + " - " + code2FA + "\n");
            string[] winPos = new string[] {
                "0,0",
                "300,0",
                "600,0",
                "900,0",
                "1200,0",
                "1500,0",
                "0,300",
                "300,300",
                "600,300",
                "900,300",
                "1200,300",
                "1500,300",
                "0,600",
                "300,600",
                "600,600",
                "900,600",
                "1200,600",
                "1500,600",
                "0,900",
                "300,900",
                "600,900",
                "900,900",
                "1200,900",
                "1500,900",
            };

            string profileId = "";

            string remoteAddress = "";
            try
            {
                profileId = createNewProfile(email);

                GPMLoginApiV3 api = new GPMLoginApiV3(txtGPMAPI.Text, winPos[windows - 1], "300,300");

                addItemToListView(email, email, "Start");

                JObject startResultObj = await api.StartProfileAsync(profileId);

                await Task.Delay(5000);

                FileInfo gpmDriverFileInfo = new FileInfo(Convert.ToString(startResultObj["data"]["driver_path"]));
                remoteAddress = Convert.ToString(startResultObj["data"]["remote_debugging_address"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động Chrome Profile.\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Puppeteer
            string remoteBrowserEndpoint = "http://" + remoteAddress;
            ConnectOptions pupLaunchOptions = new ConnectOptions()
            {
                BrowserURL = remoteBrowserEndpoint,
            };
            var browser = await Puppeteer.ConnectAsync(pupLaunchOptions);

            using (var page = await browser.NewPageAsync())
            {
                bool isContinue = true;
                string status = "Fail";

                var navigationOptions = new NavigationOptions
                {
                    Timeout = 60000
                };

                try
                {
                    await NavigateWithRetry(page, "https://www.amazon.com/ap/signin?openid.pape.max_auth_age=0&openid.return_to=https%3A%2F%2Famazon.com%2Fbusiness%2Fregister%2Faccount-setup%2Fconvert%3Fabreg_signature%3DA2v2Mmcr4DCN1srbagmSG4KWWZIX1vRa1SMiRgJWrQ0%253D%26abreg_entryRefTag%3Db2b_mcs_header_primarycta%26abreg_ingressFlow%3DNONE%26abreg_client%3Dbiss%26ref_%3Dab_reg_notag_bl_cv_ab_reg_dsk%26alreadyVisitedAuthPortal%3D1%26abreg_layoutOverride%3DSTANDALONE&openid.identity=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.assoc_handle=amzn_ab_reg_web_us&openid.mode=checkid_setup&marketPlaceId=ATVPDKIKX0DER&language=en_US&openid.claimed_id=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&pageId=ab_registration_default_desktop&openid.ns=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0&ref_=ab_reg_notag_bl_ap-sn_ab_reg_dsk&openid.pape.preferred_auth_policies=Singlefactor&switch_account=signin&disableLoginPrepopulate=1", navigationOptions);
                    await Task.Delay(2000);
                }
                catch (Exception ex)
                {
                    addItemToListView(email, email, "DIE");
                    _totalAccDie++;
                    isContinue = false;
                    status += " - Internet Error (1)";
                }

                if (isContinue)
                {
                    var continueButton = await page.QuerySelectorAsync("button[type='submit'][alt='Continue shopping']");
                    if (continueButton != null)
                    {
                        await continueButton.ClickAsync();
                        await Task.Delay(2000);
                    }
                }

                if (isContinue)
                {
                    // Login
                    await PerformActionWithWait(page, "input#ap_email", async () => await page.TypeAsync("input#ap_email", email), true);
                    await PerformActionWithWait(page, "input#ap_password", async () => await page.TypeAsync("input#ap_password", password), true);
                    await PerformActionWithWait(page, "input#signInSubmit", async () => await page.ClickAsync("input#signInSubmit"));
                }

                if (isContinue)
                    await PerformActionWithWait(page, "h4.a-alert-heading", async () =>
                    {
                        var alertHeading = await page.QuerySelectorAsync("h4.a-alert-heading");
                        var alertHeadingContent = await page.EvaluateFunctionAsync<string>("el => el.textContent", alertHeading);
                        if (alertHeadingContent.Length != 0)
                        {
                            if (alertHeadingContent.Contains("problem"))
                            {
                                addItemToListView(email, email, "DIE - PWD");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - WRONG PASSWORD (1)";
                            }
                        }
                    }, false, 1500);

                //if (isContinue)
                //    await PerformActionWithWait(page, "h1#aacb-captcha-header", async () =>
                //    {
                //        var alertHeading = await page.QuerySelectorAsync("h1#aacb-captcha-header");
                //        var alertHeadingContent = await page.EvaluateFunctionAsync<string>("el => el.textContent", alertHeading);
                //        if (alertHeadingContent.Length != 0)
                //        {
                //            addItemToListView(email, email, "DIE");
                //            _totalAccDie++;
                //            isContinue = false;
                //            status += " - " + alertHeadingContent;
                //        }
                //    }, false, 1500);



                if (isContinue)
                    // Wait for 2FA input field to appear
                    await PerformActionWithWait(page, "input#auth-mfa-otpcode", async () =>
                    {
                        string otp = get2FACode(code2FA);
                        await page.TypeAsync("input#auth-mfa-otpcode", otp);
                        await page.ClickAsync("input#auth-mfa-remember-device");
                        await page.ClickAsync("input#auth-signin-button");
                    });
                if (isContinue)
                {
                    await PerformActionWithWait(page, "h4.a-alert-heading", async () =>
                    {
                        var alertHeading = await page.QuerySelectorAsync("h4.a-alert-heading");
                        var alertHeadingContent = await page.EvaluateFunctionAsync<string>("el => el.textContent", alertHeading);
                        if (alertHeadingContent.Length != 0)
                        {
                            if (alertHeadingContent.Contains("problem"))
                            {
                                addItemToListView(email, email, "DIE");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - WRONG PASSWORD (2)";
                            }
                            if (alertHeadingContent.Contains("locked"))
                            {
                                addItemToListView(email, email, "DIE");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - Account Locked (1)";
                            }
                            if (alertHeadingContent.Contains("temporarily"))
                            {
                                addItemToListView(email, email, "DIE");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - Temporarily (1)";
                            }
                            if (alertHeadingContent.Contains("verification"))
                            {
                                addItemToListView(email, email, "DIE");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - Verification (1)";
                            }
                        }
                    }, false, 1500);
                }

                if (isContinue)
                {
                    try
                    {
                        await PerformActionWithWait(page, "text=Which account do you want to use?", async () =>
                        {
                            await PerformActionWithWait(page, "#Primary\\.REGISTRATION_START_MANAGE_REGISTRATION\\.redirect", async () => await page.ClickAsync("#Primary\\.REGISTRATION_START_MANAGE_REGISTRATION\\.redirect"));

                            await PerformActionWithWait(page, "button#redirectButton", async () => await page.ClickAsync("button#redirectButton"));

                        }, false, 5000);

                        await PerformActionWithWait(page, "button#Primary\\.REGISTRATION_START_CREATE_ACCOUNT\\.create_shuma_account", async () => await page.ClickAsync("button#Primary\\.REGISTRATION_START_CREATE_ACCOUNT\\.create_shuma_account"), false, 5000);


                        await PerformActionWithWait(page, "#Primary\\.REGISTRATION_START_COMPLETE_REGISTRATION\\.redirect", async () => await page.ClickAsync("#Primary\\.REGISTRATION_START_COMPLETE_REGISTRATION\\.redirect"), false, 5000);


                        await PerformActionWithWait(page, "text=Enter your business details", async () =>
                        {
                            string randomNumber = GenerateRandomUSPhoneNumber();
                            string randomAddress = GenerateRandomUSAddress();
                            string randomFullname = GenerateRandomUSFullName();
                            string randomBusinessName = GenerateRandomBusinessName();

                            await PerformActionWithWait(page, "input#voice-field-id", async () =>
                            {
                                var input = await page.QuerySelectorAsync("input#voice-field-id");
                                if (input != null)
                                {
                                    await input.FocusAsync();
                                    await page.Keyboard.DownAsync("Control");
                                    await page.Keyboard.PressAsync("A");
                                    await page.Keyboard.UpAsync("Control");
                                    await page.Keyboard.PressAsync("Backspace");
                                }
                            }, false);

                            await PerformActionWithWait(page, "input#voice-field-id", async () => await page.TypeAsync("input#voice-field-id", randomNumber), true);

                            await PerformActionWithWait(page, "input#businessName-field-id", async () =>
                            {
                                var input = await page.QuerySelectorAsync("input#businessName-field-id");
                                if (input != null)
                                {
                                    await input.FocusAsync();
                                    await page.Keyboard.DownAsync("Control");
                                    await page.Keyboard.PressAsync("A");
                                    await page.Keyboard.UpAsync("Control");
                                    await page.Keyboard.PressAsync("Backspace");
                                }
                            }, false);

                            await PerformActionWithWait(page, "input#businessName-field-id", async () => await page.TypeAsync("input#businessName-field-id", randomBusinessName), true);

                            await PerformActionWithWait(page, "input[name='businessType'][value='OTHER']", async () => await page.ClickAsync("input[name='businessType'][value='OTHER']"));

                            await PerformActionWithWait(page, "#businessInfoFormId > div.b-mv-small > div > span:nth-child(2) > button", async () => await page.ClickAsync("#businessInfoFormId > div.b-mv-small > div > span:nth-child(2) > button"));

                            try
                            {
                                await PerformActionWithWait(page, "a.b-clickable.b-text-carbon.b-text-truncate.b-ph-small", async () => await page.ClickAsync("a.b-clickable.b-text-carbon.b-text-truncate.b-ph-small"));

                                await Task.Delay(2000);

                                await PerformActionWithWait(page, "button#redirectButton", async () => await page.ClickAsync("button#redirectButton"));
                            }
                            catch (Exception ex)
                            {
                                addItemToListView(email, email, "DIE - Unable to add address");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - Unable to add address (1)";
                            }

                            await Task.Delay(3000);


                            if (isContinue)
                            {
                                await PerformActionWithWait(page, "button#business-info-page-submit", async () => await page.ClickAsync("button#business-info-page-submit"));
                            }
                            await Task.Delay(3000);

                        }, false, 5000);
                    }
                    catch (Exception ex)
                    {
                        addItemToListView(email, email, "DIE");
                        _totalAccDie++;
                        isContinue = false;
                        status += " - LOGIN FAILED (1)";
                    }
                }

                // Remove all current payment methods
                if (isContinue && cbDelAllOldCard.Checked)
                {
                    try
                    {
                        await NavigateWithRetry(page, "https://www.amazon.com/cpe/yourpayments/wallet", navigationOptions);
                        await Task.Delay(2000);
                    }
                    catch (Exception ex)
                    {
                        addItemToListView(email, email, "DIE");
                        _totalAccDie++;
                        isContinue = false;
                        status += " - Internet Error (2)";
                    }

                    if (isContinue)
                    {
                        await NavigateWithRetry(page, "https://www.amazon.com/cpe/yourpayments/wallet", navigationOptions);
                        await Task.Delay(2000);
                        var logoLinkElement = await page.QuerySelectorAsync("a.nav-logo-link");
                        if (logoLinkElement != null)
                        {
                            var ariaLabel = await page.EvaluateFunctionAsync<string>("el => el.getAttribute('aria-label')", logoLinkElement);
                            if (string.IsNullOrEmpty(ariaLabel) || !ariaLabel.Contains("Business"))
                            {
                                await NavigateWithRetry(page, "https://www.amazon.com/ap/signin?openid.pape.max_auth_age=0&openid.return_to=https%3A%2F%2Fwww.amazon.com%2Fgp%2Fcss%2Fhomepage.html%3Fref_%3Dnav_youraccount_switchacct&openid.identity=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.assoc_handle=usflex&openid.mode=checkid_setup&marketPlaceId=ATVPDKIKX0DER&openid.claimed_id=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.ns=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0&switch_account=picker&ignoreAuthState=1&_encoding=UTF8", navigationOptions);
                                await Task.Delay(2000);
                                await ClickLastSwitchAccountRequestAsync(page);
                                await Task.Delay(2000);
                                await NavigateWithRetry(page, "https://www.amazon.com/cpe/yourpayments/wallet", navigationOptions);
                                await Task.Delay(2000);
                            }
                        }

                        var countPaymentMethods = await page.EvaluateExpressionAsync<int>("document.querySelectorAll('div.apx-wallet-desktop-payment-method-selectable-tab-inner-css div.apx-wallet-selectable-payment-method-tab').length");
                        for (int i = 0; i < countPaymentMethods; i++)
                        {
                            try
                            {
                                await NavigateWithRetry(page, "https://www.amazon.com/cpe/yourpayments/wallet", navigationOptions);
                                await Task.Delay(1000);
                            }
                            catch (Exception ex)
                            {
                                addItemToListView(email, email, "DIE");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - Internet Error (6)";
                                break;
                            }
                            await PerformActionWithWait(page, "a[href='#'][class='a-link-normal']", async () => await page.ClickAsync("a[href='#'][class='a-link-normal']"));
                            await Task.Delay(2000);
                            await PerformActionWithWait(page, "input.apx-remove-link-button", async () => await page.ClickAsync("input.apx-remove-link-button"));
                            await Task.Delay(2000);
                            await PerformActionWithWait(page, "input.a-button-input", async () => await page.ClickAsync("span.pmts-delete-instrument input"));
                            await Task.Delay(1500);
                        }
                    }
                }

                // Create a array of links
                string[] links = this._listURL;

                string[] cardAdded = new string[] { };

                string outputAccount = $"{email}\t{password}\t{code2FA}\t{status}";
                // Add new payment methods
                if (isContinue && cbAddCard.Checked)
                {
                    int countCard = 0;
                    string outputCard = "";
                    if (countCard == nudCard.Value)
                    {
                        return;
                    }
                    for (int i = 0; i < nudCard.Value; i++)
                    {
                        if (this._listCard.Length == 0)
                        {
                            break;
                        }
                        
                        Random random = new Random();
                        try
                        {
                            await NavigateWithRetry(page, links[random.Next(links.Length)], navigationOptions);
                            await Task.Delay(1000);
                        }
                        catch (Exception ex)
                        {
                            addItemToListView(email, email, "DIE");
                            _totalAccDie++;
                            isContinue = false;
                            status += " - Internet Error (3)";
                            break;
                        }

                        if (isContinue)
                        {
                            try
                            {
                                await Task.Delay(2000);
                                var menuTextElement = await page.QuerySelectorAsync("span.menuTextSelectedMobile");
                                var menuTextContent = await page.EvaluateFunctionAsync<string>("el => el.textContent", menuTextElement);
                                if (menuTextContent.Length != 0)
                                {
                                    if (!menuTextContent.Contains("Wallet"))
                                    {
                                        addItemToListView(email, email, "DIE - Not Wallet");
                                        _totalAccDie++;
                                        isContinue = false;
                                        status += " - Not Wallet (1)";
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                addItemToListView(email, email, "DIE - Not Wallet");
                                _totalAccDie++;
                                isContinue = false;
                                status += " - Not Wallet (2)";
                                break;
                            }
                        }

                        if (isContinue)
                        {
                            countCard++;
                            string card = this._listCard[0];
                            cardAdded = cardAdded.Append(card).ToArray();
                            this._listCard = this._listCard.Skip(1).ToArray();
                            lblCard.Text = this._listCard.Length.ToString();
                            IOFile.WriteAllLines(txtListCard.Text, this._listCard);
                            string[] cardInfo = card.Split('|');
                            string cardNumber = cardInfo[0];
                            string cardMonth = cardInfo[1];
                            if (cardMonth.StartsWith("0"))
                            {
                                cardMonth = cardMonth.Substring(1);
                            }
                            string cardYear = cardInfo[2];

                            await Task.Delay(2000);
                            await PerformActionWithWait(page, "a.apx-wallet-add-link", async () => await page.ClickAsync("a.apx-wallet-add-link"));
                            await Task.Delay(2000);

                            await PerformActionWithWait(page, "span.a-button-text", async () =>
                            {
                                var addCardElements = await page.QuerySelectorAllAsync("span.a-button-text");
                                foreach (var element in addCardElements)
                                {
                                    var textContent = await page.EvaluateFunctionAsync<string>("el => el.textContent", element);
                                    if (textContent.Contains("Add a credit or debit card"))
                                    {
                                        await element.ClickAsync();
                                        break;
                                    }
                                }
                            });
                            await Task.Delay(5000);

                            await TypeActionInIframe(page, "iframe.apx-secure-iframe.pmts-portal-component", "input.pmts-account-Number", cardNumber, email);
                            await TypeActionInIframe(page, "iframe.apx-secure-iframe.pmts-portal-component", "input.apx-add-credit-card-account-holder-name-input", GenerateRandomUSFullName(), email);
                            await SelectActionInIframe(page, "iframe.apx-secure-iframe.pmts-portal-component", "select[name='ppw-expirationDate_month']", cardMonth);
                            await SelectActionInIframe(page, "iframe.apx-secure-iframe.pmts-portal-component", "select[name='ppw-expirationDate_year']", cardYear);
                            await ClickActionInIframe(page, "iframe.apx-secure-iframe.pmts-portal-component", "input[name='ppw-widgetEvent:AddCreditCardEvent']");

                            outputCard += $"{card},";
                        }
                    }

                    if (isContinue)
                    {
                        status = "Success";
                        addItemToListView(email, email, "PASS");
                        _totalAccPass++;
                        outputAccount = $"{email}\t{password}\t{code2FA}\t{outputCard}";
                    }
                    else
                    {
                        _totalAccDie++;
                        outputAccount = $"{email}\t{password}\t{code2FA}\t{status}";
                    }

                }

                // Check all card in here
                if (isContinue)
                {
                    await Task.Delay((int)nudDelay.Value * 1000);
                    try
                    {
                        await NavigateWithRetry(page, "https://www.amazon.com/cpe/yourpayments/wallet", navigationOptions);
                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        addItemToListView(email, email, "DIE");
                        _totalAccDie++;
                        isContinue = false;
                        status += " - Internet Error (5)";
                    }

                    string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                    if (isContinue)
                    {
                        var paymentMethods = await page.EvaluateFunctionAsync<string[]>("() => Array.from(document.querySelectorAll('div.apx-wallet-desktop-payment-method-selectable-tab-inner-css div.apx-wallet-selectable-payment-method-tab')).map(e => e.outerHTML)");
                        foreach (var paymentMethod in paymentMethods)
                        {
                            // Get 4 last digits of card
                            string cardSiteNumber = paymentMethod.Substring(paymentMethod.IndexOf("••••") + 5, 4);
                            string cardSiteName = Regex.Match(paymentMethod, "<span class=\"a-size-base apx-wallet-tab-pm-name-text a-text-bold\">(.*?)</span>").Groups[1].Value;

                            if (cardAdded.Any(c => c.Contains(cardSiteNumber)))
                            {
                                bool isFound = false;
                                foreach (var image in this._listImage)
                                {
                                    if (paymentMethod.ToLower().Contains(image.ToLower())) { isFound = true; break; }
                                }
                                if (!isFound)
                                {
                                    _totalCardLive++;
                                    string cardInfo = cardAdded.First(c => c.Contains(cardSiteNumber));
                                    IOFile.AppendAllText("./_output/" + dateNow + "-CardLIVE.txt", cardInfo + "|" + cardSiteName + "\n");
                                }
                                else
                                {
                                    _totalCardDie++;
                                    string cardInfo = cardAdded.First(c => c.Contains(cardSiteNumber));
                                    IOFile.AppendAllText("./_output/" + dateNow + "-CardDIE.txt", cardInfo + "|" + cardSiteName + "\n");
                                }
                            }

                        }
                    }

                }

                await Task.Delay(1500);
                closeProfile(profileId);
                await Task.Delay(1000);
                deleteProfile(profileId);
                this._totalWindows--;
                // Check folder _output exist
                if (!Directory.Exists("_output"))
                {
                    Directory.CreateDirectory("_output");
                }
                string date = DateTime.Now.ToString("yyyy-MM-dd");

                if (status == "Success" || status.Contains("Success"))
                {
                    IOFile.AppendAllText("./_output/" + date + "-PASS.txt", outputAccount + "\n");
                }
                else
                {
                    IOFile.AppendAllText("./_output/" + date + "-ERROR.txt", outputAccount + "\n");
                }

            }
        }
        private string GenerateRandomUSPhoneNumber()
        {
            Random random = new Random();
            string[] areaCodeArray = { "201", "202", "203", "204", "205", "206", "207", "208", "209", "210", "212", "213", "214", "215", "216", "217", "218", "219", "224", "225", "228", "229", "231", "234", "239", "240", "242", "246", "248", "250", "251", "252", "253", "254", "256", "260", "262", "264", "267", "268", "269", "270", "276", "281", "284", "289", "301", "302", "303", "304", "305", "306", "307", "308", "309", "310", "312", "313", "314", "315", "316", "317", "318", "319", "320", "321", "323", "325", "330", "334", "336", "337", "339", "340", "345", "347", "351", "352", "360", "361", "386", "401", "402", "403", "404", "405", "406", "407", "408", "409", "410", "412", "413", "414", "415", "416", "417", "418", "419", "423", "425", "430", "432", "434", "435", "440", "441", "443", "450", "456", "469", "473", "478", "479", "480", "484", "501", "502", "503", "504", "505", "506", "507", "508", "509", "510", "512", "513", "514", "515", "516", "517", "518", "519", "520", "530", "540", "541", "551", "559", "561", "562", "563", "567", "570", "571", "573", "574", "580", "585", "586", "601", "602", "603", "604", "605", "606", "607", "608", "609", "610", "612", "613", "614", "615", "616", "617", "618", "619", "620", "623", "626", "630", "631", "636", "641", "646", "647", "649", "650", "651", "660", "661", "662", "664", "670", "671", "678", "682", "701", "702", "703", "704", "705", "706", "707", "708", "709", "710", "712", "713", "714", "715", "716", "717", "718", "719", "720", "724", "727", "731", "732", "734", "740", "754", "757", "758", "760", "763", "765", "767", "770", "772", "773", "774", "775", "778", "780", "781", "784", "785", "786", "787", "801", "802", "803", "804", "805", "806", "807", "808", "809", "810", "812", "813", "814", "815", "816", "817", "818", "819", "828", "830", "831", "832", "843", "845", "847", "848", "850", "856", "857", "858", "859", "860", "862", "863", "864", "865", "867", "868", "869", "870", "876", "878", "880", "881", "882", "901", "902", "903", "904", "905", "906", "907", "908", "909", "910", "912", "913", "914", "915", "916", "917", "918", "919", "920", "925", "928", "931", "936", "937", "939", "940", "941", "947", "949", "952", "954", "956", "970", "971", "972", "973", "978", "979", "980", "985", "989" };
            int areaCode = int.Parse(areaCodeArray[random.Next(areaCodeArray.Length)]);

            int centralOfficeCode = random.Next(200, 999);
            int lineNumber = random.Next(1000, 9999);

            return $"({areaCode}) {centralOfficeCode}-{lineNumber}";
        }

        private string GenerateRandomUSAddress()
        {
            Random random = new Random();
            int streetNumber = random.Next(1, 1000);
            char streetLetter = (char)random.Next('A', 'Z' + 1);

            return $"{streetNumber} {streetLetter}";
        }


        private string get2FACode(string secretCode)
        {
            var secretKey = Base32Encoding.ToBytes(secretCode);
            var totp = new Totp(secretKey);
            var otp = totp.ComputeTotp();
            bool isValid = totp.VerifyTotp(otp, out long timeStepMatched);
            if (isValid)
            {
                return otp;
            }
            return null;
        }


        public class Profile
        {
            public string id { get; set; }
            public string name { get; set; }
            public string raw_proxy { get; set; }
            public string profile_path { get; set; }
            public string browser_type { get; set; }
            public string browser_version { get; set; }
            public string note { get; set; }
            public int group_id { get; set; }
            public DateTime created_at { get; set; }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!checkAllRequirements())
            {
                return;
            }
        }

        private bool checkAllRequirements()
        {
            if (txtGPMAPI.Text == "" || txtListCard.Text == "" || txtAMZAccount.Text == "" || txtURL.Text == "" || txtImageDie.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!IOFile.Exists(txtListCard.Text))
            {
                IOFile.Create(txtListCard.Text).Close();
            }
            if (!IOFile.Exists(txtAMZAccount.Text))
            {
                IOFile.Create(txtAMZAccount.Text).Close();
            }
            if (!IOFile.Exists(txtURL.Text))
            {
                IOFile.Create(txtURL.Text).Close();
            }
            if (!IOFile.Exists(txtImageDie.Text))
            {
                IOFile.Create(txtImageDie.Text).Close();
            }

            if (!Directory.Exists("_output"))
            {
                Directory.CreateDirectory("_output");
            }
            if (!Directory.Exists("_input"))
            {
                Directory.CreateDirectory("_input");
            }

            string[] cardLines = IOFile.ReadAllLines(txtListCard.Text);
            this._listCard = cardLines;
            lblCard.Text = cardLines.Length.ToString();
            string[] accLines = IOFile.ReadAllLines(txtAMZAccount.Text);
            this._listAccount = accLines;
            lblAccount.Text = accLines.Length.ToString();
            string[] imageFileRead = IOFile.ReadAllLines(txtImageDie.Text);
            this._listImage = imageFileRead;
            if (cardLines.Length <= 0 || accLines.Length <= 0 || imageFileRead.Length <= 0)
            {
                timer1.Stop();
                MessageBox.Show("File không có dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", txtListCard.Text);
        }

        private void btnAddAcc_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", txtAMZAccount.Text);
        }
        private async Task<T> RetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3, int delayMilliseconds = 1000)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (retryCount < maxRetries)
                {
                    retryCount++;
                    await Task.Delay(delayMilliseconds);
                }
            }
        }

        private async Task NavigateWithRetry(IPage page, string url, NavigationOptions options, int maxRetries = 3, int delayMilliseconds = 1000)
        {
            int attempt = 0;
            while (attempt < maxRetries)
            {
                try
                {
                    if (page == null)
                    {
                        // Log the error or handle it as needed
                        Console.WriteLine("Page object is null. Skipping navigation.");
                        return;
                    }
                    else
                    {
                        await page.GoToAsync(url, options);
                        return; // Navigation succeeded, exit the method

                    }
                }
                catch (NavigationException ex)
                {
                    if (ex.Message.Contains("net::ERR_TIMED_OUT"))
                    {
                        attempt++;
                        if (attempt >= maxRetries)
                        {
                            throw; // Rethrow the exception if max retries reached
                        }
                        await Task.Delay(delayMilliseconds); // Wait before retrying
                    }
                    else
                    {
                        throw; // Rethrow the exception if it's not a timeout error
                    }
                }
            }
        }

        private void txtProxy_TextChanged(object sender, EventArgs e)
        {
            //SaveConfig();
        }

        private void btnAddURL_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", txtURL.Text);
        }

        private void btnSetImageList_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", txtImageDie.Text);
        }
        // Add this method to your Form1 class
        public async Task ClickLastSwitchAccountRequestAsync(IPage page)
        {
            // Wait for at least one element to appear
            await page.WaitForSelectorAsync("a[data-name='switch_account_request']");

            // Query all matching elements
            var elements = await page.QuerySelectorAllAsync("a[data-name='switch_account_request']");

            if (elements != null && elements.Length > 0)
            {
                // Click the last element in the array
                await elements[elements.Length - 1].ClickAsync();
            }
            else
            {
                throw new Exception("No elements with data-name='switch_account_request' found.");
            }
        }
    }
}
