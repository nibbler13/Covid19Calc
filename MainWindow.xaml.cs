using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Covid19Calc {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public List<string> Logistics { get; set; } = new List<string> { "В пределах МКАД", "В пределах 10 км от МКАД", "В пределах 30 км от МКАД" };
		public List<int> PatientsCount { get; set; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		private string costTotal;
		public string CostTotal { 
			get { return costTotal; }
			set {
				if (value != costTotal) {
					costTotal = value;
					NotifyPropertyChanged();
				}
			}
		}


		private string selectedLogistics;
		public string SelectedLogistics {
			get { return selectedLogistics; }
			set {
				if (value != selectedLogistics) {
					selectedLogistics = value;
					NotifyPropertyChanged();
					UpdateServices();
				}
			}
		}

		private int selectedAdultCount;
		public int SelectedAdultCount {
			get { return selectedAdultCount; }
			set {
				if (value != selectedAdultCount) {
					selectedAdultCount = value;
					NotifyPropertyChanged();
					UpdateServices();
				}
			}
		}

		private int selectedKidsCount;
		public int SelectedKidsCount {
			get { return selectedKidsCount; }
			set {
				if (value != selectedKidsCount) {
					selectedKidsCount = value;
					NotifyPropertyChanged();
					UpdateServices();
				}
			}
		}

		private Brush costTotalBackground = new SolidColorBrush(Colors.Transparent);
		public Brush CostTotalBackground {
			get { return costTotalBackground; }
			set {
				if (value != costTotalBackground) {
					costTotalBackground = value;
					NotifyPropertyChanged();
				}
			}
		}

		public ObservableCollection<ItemService> Services { get; set; } = new ObservableCollection<ItemService>();

		public string PatientName { get; set; }
		public string PatientPhoneNumber { get; set; }
		public string PatientAddress { get; set; }
		public DateTime DesiredDate { get; set; } = DateTime.Now;
		public string Comment { get; set; }

		public MainWindow() {
			InitializeComponent();
			DataContext = this;
		}


		private void UpdateServices() {
			bool isError = false;
			if (string.IsNullOrEmpty(SelectedLogistics)) {
				CostTotal = "не выбрано местонахождение";
				isError = true;
			}

			if (SelectedAdultCount + SelectedKidsCount == 0) {
				CostTotal = "не указано кол-во пациентов";
				isError = true;
			}

			if (isError) {
				CostTotalBackground = new SolidColorBrush(Colors.Yellow);
				return;
			}

			Services.Clear();

			if (SelectedAdultCount > 0) {
				switch (SelectedLogistics) {
					case "В пределах МКАД":
						Services.Add(
							new ItemService {
								Id = "1002285",
								Name = "Взятие биоматериала на ПЦР исследование на коронавирус на дому в пределах МКАД",
								Cost = 4650,
								Count = 1
							});
						break;

					case "В пределах 10 км от МКАД":
						Services.Add(
							new ItemService {
								Id = "1002286",
								Name = "Взятие биоматериала на ПЦР исследование на коронавирус на дому в пределах 10 км от МКАД",
								Cost = 6150,
								Count = 1
							});
						break;

					case "В пределах 30 км от МКАД":
						Services.Add(
							new ItemService {
								Id = "1002287",
								Name = "Взятие биоматериала на ПЦР исследование на коронавирус на дому в пределах 30 км от МКАД",
								Cost = 8150,
								Count = 1
							});
						break;

					default:
						break;
				}

				if (SelectedAdultCount > 1)
					Services.Add(
						new ItemService {
							Id = "101899",
							Name = "Взятие биоматериала на ПЦР исследование на коронавирус",
							Cost = 650,
							Count = SelectedAdultCount - 1
						});

				if (SelectedKidsCount > 0)
					Services.Add(
						new ItemService {
							Id = "212066",
							Name = "Взятие биоматериала на ПЦР исследование на коронавирус",
							Cost = 650,
							Count = SelectedKidsCount
						});

			} else if (SelectedKidsCount > 0) {
				switch (SelectedLogistics) {
					case "В пределах МКАД":
						Services.Add(
							new ItemService {
								Id = "2110635",
								Name = "Взятие биоматериала на ПЦР исследование на коронавирус на дому в пределах МКАД",
								Cost = 4650,
								Count = 1
							});
						break;
					case "В пределах 10 км от МКАД":
						Services.Add(
							new ItemService {
								Id = "2110636",
								Name = "Взятие биоматериала на ПЦР исследование на коронавирус на дому в пределах 10 км от МКАД",
								Cost = 6150,
								Count = 1
							});
						break;
					case "В пределах 30 км от МКАД":
						Services.Add(
							new ItemService {
								Id = "2110637",
								Name = "Взятие биоматериала на ПЦР исследование на коронавирус на дому в пределах 30 км от МКАД",
								Cost = 8150,
								Count = 1
							});
						break;
					default:
						break;
				}

				if (SelectedKidsCount > 1)
					Services.Add(
						new ItemService {
							Id = "212066",
							Name = "Взятие биоматериала на ПЦР исследование на коронавирус",
							Cost = 650,
							Count = SelectedKidsCount - 1
						});
			}

			Services.Add(
				new ItemService { 
					Id = "326217",
					Name = "Определение РНК возбудителя коронавирусной инфекции COVID-19 у лиц, " +
						"не имеющих признаков инфекционного заболевания и не находящихся в прямом " + 
						"контакте с больным новой коронавирусной инфекцией, методом ПЦР", 
					Cost = 1350, 
					Count = SelectedAdultCount + SelectedKidsCount });

			int cost = 0;
			foreach (ItemService item in Services)
				cost += item.Cost * item.Count;

			CostTotalBackground = new SolidColorBrush(Colors.LightGreen);
			CostTotal = cost.ToString("N0", CultureInfo.CurrentCulture) + " руб.";
		}

		private void ButtonSend_Click(object sender, RoutedEventArgs e) {
			string error = "";

			if (DesiredDate.Date < DateTime.Now.Date)
				error = "Дата выполнения не может быть в прошедшем времени";

			if (string.IsNullOrEmpty(PatientName) ||
				string.IsNullOrEmpty(PatientPhoneNumber) ||
				string.IsNullOrEmpty(PatientAddress))
				error = "Не заполены обязательные поля (отмечены звездочкой)";

			if (string.IsNullOrEmpty(CostTotal) || !CostTotal.Contains("руб"))
				error = "Не выполнен расчет стоимости";

			if (!string.IsNullOrEmpty(error)) {
				MessageBox.Show(this, error, "Невозможно отправить заявку", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			string subject = "Заявка на выполнение теста на Covid-19";
			string body =
				"<table border=\"1\">" +
				"<tr><td>ФИО пациента</td><td><b>" + PatientName + "</b></td></tr>" +
				"<tr><td>Контактный номер телефона</td><td><b>" + PatientPhoneNumber + "</b></td></tr>" +
				"<tr><td>Адрес вызова</td><td><b>" + PatientAddress + "</b></td></tr>" +
				"<tr><td>Желаемая дата выполнения</td><td><b>" + DesiredDate.ToShortDateString() + "</b></td></tr>" +
				"<tr><td>Комментарий</td><td>" + Comment + "</td></tr>" +
				"<tr><td>Автор заявки</td><td>" + Environment.UserName + "</td></tr>" +
				"<tr><td>Время создания</td><td>" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "</td></tr></table>" +
				Environment.NewLine + Environment.NewLine +
				"<table border=\"1\"><tr><td>Местонахождение</td><td>Кол-во взрослых</td><td>Кол-во детей</td></tr>" +
				"<tr><th>" + SelectedLogistics + "</th><th><p align=\"center\">" + SelectedAdultCount +
				"</p></th><th><p align=\"center\">" + SelectedKidsCount + "</p></th></tr></table>" +
				Environment.NewLine + Environment.NewLine +
				"<table border=\"1\">" +
				"<caption>Необходимые услуги</caption>" +
				"<tr><th>Код</th><th>Наименование</th><th>Стоимость</th><th>Количество</th></tr> ";

			foreach (ItemService item in Services) 
				body += "<tr><td>" + item.Id + "</td><td>" + item.Name + "</td><td>" + item.Cost + "</td><td>" + item.Count + "</td></tr>";

			body += "<tr><td colspan=\"4\">Стоимость итого: " + CostTotal +"</td></tr></table>";

			string receiver = Properties.Settings.Default.MailTo;
			SystemMail.SendMail(subject, body, receiver);
		}
	}
}
