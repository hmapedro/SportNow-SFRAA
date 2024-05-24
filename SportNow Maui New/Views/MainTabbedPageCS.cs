using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using SportNow.Views.Profile;
using Microsoft.Maui;
using SportNow.Views.CompleteRegistration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Plugin.BetterFirebasePushNotification;
using SportNow.Views.Services;
//using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
#if IOS
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Platform;
#endif

namespace SportNow.Views
{
    public class MainTabbedPageCS : Microsoft.Maui.Controls.TabbedPage
    {

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

#if IOS
            if (args.NewHandler is TabbedRenderer renderer)
            {
                if (renderer.TabBar is not null)
                {
                    renderer.TabBar.TintColor = App.normalTextColor.ToPlatform();
                }
            }
#endif
        }

        public async void initSpecificLayout(string actiontype, string actionid) {
            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            NavigationPage.SetBackButtonTitle(this, "");

            //

            //NavigationPage.SetHasNavigationBar(this, false);

            //Add(item: new IntroPageCS() { Title = "PERFIL", IconImageSource = "perfil.png" });
            //Add(new IntroPageCS() { Title = "PRESENÇAS", IconImageSource = "presencasicon.png" });
            Children.Add(new IntroPageCS() { Title = "A CARREGAR", IconImageSource = "iconlogo.png" });
            //Add(new IntroPageCS() { Title = "EVENTOS", IconImageSource = "eventos.png" });
            //Add(new IntroPageCS() { Title = "EQUIPAMENTOS", IconImageSource = "kimono.png" });
            CurrentPage = Children[0];


            //On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            this.BackgroundColor = App.backgroundColor;
            this.BarBackgroundColor = App.backgroundColor;
            this.BarTextColor = App.topColor;// App.normalTextColor;//FromRgb(75, 75, 75);
            this.SelectedTabColor = App.inactiveTitleTextColor;
            this.UnselectedTabColor = App.bottomColor; // App.bottomColor;


            //public static double ScreenWidth; = Application.Current.MainPage.Width;
            //public static double ScreenHeight; = Application.Current.MainPage.Height;

            App.screenWidthAdapterCalculator();
            App.screenHeightAdapterCalculator();

            await checkMinimumVersion();

            if (App.members == null) //não foi feito login ainda
            {
                Debug.Print("não foi feito login ainda");
                App.members = await GetMembers();
                if (App.members == null)//com os dados que temos de login não retornou nenhum membro, logo vai para Login
                {
                    Debug.Print("App members é null");
                    Navigation.InsertPageBefore(new LoginPageCS(""), this);
                    await Navigation.PopAsync();
                    return;
                }
                else if (App.members.Count == 0) //com os dados que temos de login não retornou nenhum membro, logo vai para Login
                {
                    Debug.Print("App members é null");
                    Navigation.InsertPageBefore(new LoginPageCS(""), this);
                    await Navigation.PopAsync();
                    return;
                }
                else if (App.members.Count == 1) //foi feito login automático e retornou apenas 1 socio pelo que pode entrar para a página principal
                {
                    Debug.Print("App.members.Count = 1");
                    App.original_member = App.members[0];
                    App.member = App.original_member;

                }
                else if (App.members.Count > 1) //foi feito login automático e retornou mais do que 1 socio
                {
                    Debug.Print("App.members.Count > 1");
                    //if (App.Current.Properties.ContainsKey("SELECTEDUSER")) //vê se tem selected user gravado
                    if (Preferences.Default.Get("SELECTEDUSER", "") != "") //vê se tem selected user gravado
                    {

                        string selectedUser = Preferences.Default.Get("SELECTEDUSER", "");//Application.Current.Properties["SELECTEDUSER"] as string;
                        Debug.Print("App.members.Count > 1 e selectedUser = "+ selectedUser);
                        foreach (Member member in App.members)
                        {
                            if (selectedUser == member.id)
                            {
                                App.original_member = member;
                                App.member = member;
                            }
                        }
                        if (App.member == null) //não encontrou o selected user nos membros do login pelo que tem de ir para a página de select member
                        {
                            Debug.Print("App.members.Count > 1 e selectedUser não foi encontrado");
                            Navigation.InsertPageBefore(new SelectMemberPageCS(), this);
                            await Navigation.PopAsync();
                            return;
                        }
                    }
                    else //não tem o selected user gravado pelo que tem de ir para a página de select member
                    {
                        Debug.Print("App.members.Count > 1 e selectedUser não existe");
                        Navigation.InsertPageBefore(new SelectMemberPageCS(), this);
                        await Navigation.PopAsync();
                        return;
                    }
                }

                //await checkObjectives();

                //se chegou aqui é pq sabe quem é o member
                if ((BetterFirebasePushNotification.Current.Token!= null) & (App.original_member.id == App.member.id))
                {
                    Debug.Print("OLA1 " + BetterFirebasePushNotification.Current.Token);
                    MemberManager memberManager = new MemberManager();
                    var result = await memberManager.updateToken(App.original_member.id, BetterFirebasePushNotification.Current.Token);

                    LogManager logManager = new LogManager();
                    await logManager.writeLog(App.original_member.id, App.member.id, "LOGIN OK", "Login Successful");

                }


                if (App.member.estado == "pre_inscrito")
                {
                    //Debug.Print("Membro aprovado mas ainda não ativo 1 " + App.member.aulanome);

                    /*if ((App.member.aulanome == null) & (App.member.member_type == "praticante"))
                    {
                        Debug.Print("Membro ainda não tem aula atribuida");
                        Navigation.InsertPageBefore(new LoginPageCS("Sócio ainda não tem classe atribuída"), this);
                        await Navigation.PopAsync();
                        return;
                    }*/

                    App.Current.MainPage = new NavigationPage(new BeginPageCS())
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                    };
                    return;
                }

                //result = await GetCurrentFees(App.member);
                //App.original_member.students_count = await GetMemberStudents_Count(App.original_member.id);
                //App.member.students_count = await GetMemberStudents_Count(App.member.id);

                //App.notification = App.notification + " createTabs 0 ";
                //App.notification = App.notification + " createTabs 1 "+ actiontype;
                if (actiontype != "")
                {
                    App.member = App.original_member;
                    App.original_member.students_count = await GetMemberStudents_Count(App.original_member.id);
                    App.member.students_count = App.original_member.students_count;

                    App.notification = App.notification + " actiontype != empty ";
                    await navigateToPageAsync(actiontype, actionid);

                }
                else
                {
                    App.original_member.students_count = await GetMemberStudents_Count(App.original_member.id);
                    if (App.original_member.id != App.member.id)
                    {
                        App.member.students_count = await GetMemberStudents_Count(App.member.id);
                    }
                    else
                    {
                        App.member.students_count = App.original_member.students_count;
                    }
                }

                MemberManager memberManager_1 = new MemberManager();
                App.member.members_to_approve = await memberManager_1.GetMembers_To_Approve();
                await createTabs();
                

            }
            else if (App.members.Count == 0) //os dados do login anterior não retornaram membros pelo que volta para o login
            {
                Debug.Print("App.members.Count = 0 volta para Login");
                App.Current.MainPage = new NavigationPage(new LoginPageCS(""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
            }
            else if (App.members.Count > 0) //veio do login ou de select member e retornou vários membros mas já preencheu o que tinha a preencher.
            {

                if (App.member.estado == "pre_inscrito")
                {

                   /* Debug.Print("Membro aprovado mas ainda não ativo 2 " + App.member.aulanome);

                    if ((App.member.aulanome == null) & (App.member.member_type == "praticante"))
                    {
                        Debug.Print("Membro ainda não tem aula atribuida");
                        await Navigation.PushAsync(new LoginPageCS("Sócio ainda não tem classe atribuída"));
                        Navigation.RemovePage(this);
                        return;
                    }*/


                    Debug.Print("Membro aprovado mas ainda não ativo 2  AQUI ");
                    await Navigation.PushAsync(new BeginPageCS());
                    Navigation.RemovePage(this);
                    Debug.Print("Membro aprovado mas ainda não ativo 2  AQUI 1");

                    return;
                }

                //await checkObjectives();
                //var result = await GetCurrentFees(App.member);
                //App.original_member.students_count = await GetMemberStudents_Count(App.original_member.id);
                //App.member.students_count = await GetMemberStudents_Count(App.member.id);

                if ((BetterFirebasePushNotification.Current.Token != null) & (App.original_member.id == App.member.id))
                {
                    Debug.Print("OLA0 " + BetterFirebasePushNotification.Current.Token);
                    MemberManager memberManager = new MemberManager();
                    var result = await memberManager.updateToken(App.original_member.id, BetterFirebasePushNotification.Current.Token);
                }
                App.notification = App.notification + " createTabs 0.1 "+actiontype;
                if (actiontype != "")
                {
                    App.member = App.original_member;
                    App.original_member.students_count = await GetMemberStudents_Count(App.original_member.id);
                    App.member.students_count = App.original_member.students_count;

                    App.notification = App.notification + " actiontype != empty ";
                    await navigateToPageAsync(actiontype, actionid);

                }
                else
                {
                    App.original_member.students_count = await GetMemberStudents_Count(App.original_member.id);
                    if (App.original_member.id != App.member.id)
                    {
                        App.member.students_count = await GetMemberStudents_Count(App.member.id);
                    }
                    else
                    {
                        App.member.students_count = App.original_member.students_count;
                    }
                    
                }
                MemberManager memberManager_1 = new MemberManager();
                App.member.members_to_approve = await memberManager_1.GetMembers_To_Approve();
                await createTabs();
                App.notification = App.notification + " createTabs 2 ";

            }

            //Debug.Print("AQUI NOSSA!!!! " + App.member.id);
        }

        public async Task<int> createTabs()
        {
            Children.RemoveAt(0);
            Children.Add(new ServicesPageCS() { Title = "SERVIÇOS", IconImageSource = "iconservicos.png" });
            Children.Add(new SportsPageCS() { Title = "MODALIDADES", IconImageSource = "iconmodalidades.png" });
            Children.Add(new MainPageCS() { Title = "PRINCIPAL", IconImageSource = "iconlogo.png" });
            Children.Add(new AllEventsPageCS() { Title = "EVENTOS", IconImageSource = "eventos.png" });
            Children.Add(new EquipamentTypePageCS() { Title = "LOJA", IconImageSource = "iconequipamentos.png" });
            CurrentPage = Children[2];
            Title = "PRINCIPAL";
            CurrentPageChanged += CurrentPageHasChanged;

            return 1;
        }

        public MainTabbedPageCS(string actiontype, string actionid)
        {
            Debug.Print("MainTabbedPageCS");

            initSpecificLayout(actiontype, actionid);
        }

        private async Task<int> navigateToPageAsync(string actiontype, string actionid)
        {
            //App.member = App.original_member; //QUANDO VEM DE NOTIFICAÇÃO QUERO QUE ABRA SEMPRE NO UTILIZADOR ORIGINAL

            App.notification = App.notification + " navigateToPageAsync ";
            if (actiontype == "openevent")
            {
                App.notification = App.notification + " actiontype == openevent ";
                Event event_i = await GetEvent_byID(App.member.id, actionid);
                App.notification = App.notification + " event_i = "+event_i.name;
                await Navigation.PushAsync(new DetailEventPageCS(event_i));
            }
            else if (actiontype == "opencompetition")
            {
                App.notification = App.notification + " actiontype == opencompetition ";
                await Navigation.PushAsync(new DetailCompetitionPageCS(actionid, ""));
            }
            else if (actiontype == "openexaminationsession")
            {
                App.notification = App.notification + " actiontype == openexaminationsession ";
                await Navigation.PushAsync(new ExaminationSessionPageCS(actionid));
            }
            else if (actiontype == "openweekclass")
            {
                App.notification = App.notification + " actiontype == openweekclass ";
                await Navigation.PushAsync(new AttendancePageCS("next"));
            }
            else if (actiontype == "opentodayclass")
            {
                App.notification = App.notification + " actiontype == opentodayclass ";
                await Navigation.PushAsync(new AttendancePageCS());
            }
            else if (actiontype == "opentodayclassinstructor")
            {
                App.notification = App.notification + " actiontype == opentodayclassinstructor ";
                await Navigation.PushAsync(new AttendanceManagePageCS());
            }
            else if (actiontype == "authorizeregistration")
            {
                App.notification = App.notification + " actiontype == authorizeregistration ";
                await Navigation.PushAsync(new ApproveRegistrationPageCS());
            }
            else if (actiontype == "openevaluationclass")
            {
                App.notification = App.notification + " actiontype == openevaluationclass ";
                await Navigation.PushAsync(new AttendanteEvaluationPageCS(actionid));
            }

            return 0;
        }

        async Task<Event> GetEvent_byID(string memberid, string eventid)
        {
            Debug.WriteLine("GetFutureExaminationSessions");
            EventManager eventManager = new EventManager();

            Event event_i = await eventManager.GetEvent_byID(memberid, eventid);
            if (event_i == null)
            {
                Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
            }

            return event_i;
        }


        private void CurrentPageHasChanged(object sender, EventArgs e)
        {
            Title = CurrentPage.Title;
            NavigationPage.SetBackButtonTitle(this, "");
        }


        public async Task<List<Member>> GetMembers()
        {
            Debug.WriteLine("GetMembers");

            string username = "", password = "";

            
            if (Preferences.Default.Get("EMAIL", "") != "")
            {
                username = Preferences.Default.Get("EMAIL", "");
            }

            if (Preferences.Default.Get("PASSWORD", "") != "")
            {
                password = Preferences.Default.Get("PASSWORD", "");
            }
            var user = new User
            {
                Username = username,
                Password = password
            };

            MemberManager memberManager = new MemberManager();

            List<Member> members;

            members = await memberManager.GetMembers(user);

            return members;

        }

        async Task<int> GetCurrentFees(Member member)
        {
            Debug.WriteLine("MainTabbedPageCS.GetCurrentFees");
            MemberManager memberManager = new MemberManager();

            var result = await memberManager.GetCurrentFees(member);

            return result;
        }


        async Task<int> checkMinimumVersion()
        {
            Debug.Print("MainTabbedPageCS.checkMinimumVersion");
            AppManager appManager = new AppManager();

            MinimumVersion minimumVersion = await appManager.GetMinimumVersion();

            if (minimumVersion != null)
            {
                if (Convert.ToInt32(minimumVersion.build) > Convert.ToInt32(App.BuildNumber))
                {
                    await DisplayAlert("ATUALIZAR APP", "Para continuar a utilizar a nossa App deverá efetuar a atualização para uma versão mais recente.", "Ok");

                    Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(new LoginPageCS("Efetue a atualização da App para uma versão mais recente."))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };
                }
            }
            else
            {
                Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à internet."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
            }

            
            Debug.Print("MainTabbedPageCS.checkMinimumVersion is OK!");
            return 1;
        }

        async Task<int> checkObjectives()
        {
            Debug.Print("MainTabbedPageCS.checkObjectives");
            MemberManager memberManager = new MemberManager();

            App.member.objectives = await memberManager.GetObjectives_bySeason(App.member.id, App.getSeason());

            if (App.member.objectives.Count == 0)
            {
                await Navigation.PushAsync(new ObjectivesPageCS());
            }
            return 1;
        }

        async Task<int> GetMemberStudents_Count(string memberid)
        {
            Debug.WriteLine("MainTabbedPageCS.GetMemberStudents_Count");
            MemberManager memberManager = new MemberManager();

            var result = await memberManager.GetMemberStudents_Count(memberid);

            return result;
        }
    }
}
