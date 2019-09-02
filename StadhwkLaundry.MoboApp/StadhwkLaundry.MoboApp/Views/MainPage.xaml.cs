using StadhwkLaundry.MoboApp.Models;
using StadhwkLaundry.MoboApp.Views.BeAFranchisee;
using StadhwkLaundry.MoboApp.Views.FAQ;
using StadhwkLaundry.MoboApp.Views.Home;
using StadhwkLaundry.MoboApp.Views.InviteAndEarn;
using StadhwkLaundry.MoboApp.Views.Membership;
using StadhwkLaundry.MoboApp.Views.OrderHistory;
using StadhwkLaundry.MoboApp.Views.Support;
using StadhwkLaundry.MoboApp.Views.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StadhwkLaundry.MoboApp.Views;

namespace StadhwkLaundry.MoboApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        public MainPage()
        {
            InitializeComponent();
            menuList = new List<MasterPageItem>();

            var homePage = new MasterPageItem() { id = 1, Title = "Home", Icon = "Home.png" };
            var orderHistoryPage = new MasterPageItem() { id = 2, Title = "Order History", Icon = "About.png" };
            var walletPage = new MasterPageItem() { id = 3, Title = "Wallet", Icon = "Configuration.png" };
            var membershipPage = new MasterPageItem() { id = 4, Title = "Membership", Icon = "ProfileSetting.png" };
            var inviteAndEarnPage = new MasterPageItem() { id = 5, Title = "Invite & Earn", Icon = "Configuration.png" };
            var BeAFranchiseePage = new MasterPageItem() { id = 6, Title = "Be A Franchisee", Icon = "ProfileSetting.png" };
            var supportPage = new MasterPageItem() { id = 6, Title = "Support", Icon = "ProfileSetting.png" };
            var fAQPage = new MasterPageItem() { id = 6, Title = "f&Q", Icon = "ProfileSetting.png" };

            menuList.Add(homePage);
            menuList.Add(orderHistoryPage);
            menuList.Add(walletPage);
            menuList.Add(membershipPage);
            menuList.Add(inviteAndEarnPage);
            menuList.Add(BeAFranchiseePage);
            menuList.Add(fAQPage);
            navigationDrawerList.ItemsSource = menuList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
        }


        async private void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var myselecteditem = e.Item as MasterPageItem;
            switch (myselecteditem.id)
            {

                case 1:
                    await Navigation.PushAsync(new HomePage());

                    break;
                case 2:
                    await Navigation.PushAsync(new OrderHistoryPage());

                    break;
                case 3:
                    await Navigation.PushAsync(new WalletPage());

                    break;
                case 4:
                    await Navigation.PushAsync(new MembershipPage());

                    break;
                case 5:
                    await Navigation.PushAsync(new InviteAndEarnPage());

                    break;
                case 6:
                    await Navigation.PushAsync(new SupportPage());
                    break;
                case 7:
                    await Navigation.PushAsync(new BeAFranchiseePage());
                    break;
                case 8:
                    await Navigation.PushAsync(new FAQPage());
                    break;
            }
             ((ListView)sender).SelectedItem = null;
            IsPresented = false;
        }
    }
}