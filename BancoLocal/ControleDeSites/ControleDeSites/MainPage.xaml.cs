using SQLite;

namespace ControleDeSites;

public partial class MainPage : ContentPage
{
	string dbPath; //indica onde esta o banco de dados
	SQLiteConnection conn; //representa a conexao com o banco de dados 

	public MainPage()
	{
		InitializeComponent();
        CriarBanco();
	}

    public void ListarSites()
    {
        List<Site> lista = conn.Table<Site>().ToList();
        ListaClv.ItemsSource = lista;
    }

    private void CriarBanco()
    {
		dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory,"sites.db3");
		conn = new SQLiteConnection(dbPath);
		conn.CreateTable<Site>();
		itensVsl.IsVisible = true;
        ListarSites();
    }

    private void inserirBtn_Clicked(object sender, EventArgs e)
    {
        //pegar os dados da tela
        //int id = Convert.ToInt32(idEnt.Text);
        //string endereco = siteEnt.Text;
        Site site = new Site();
        if (Uri.TryCreate(siteEnt.Text, UriKind.Absolute, out Uri urlResult))
        {
            site.Endereco = siteEnt.Text;
        }
        else
        {
            DisplayAlert("Erro", "Informe um endereço correto", "OK");
            return;
        }

        //realizar as operações
        try 
        {
            conn.Insert(site);
            idEnt.Text = "";
            siteEnt.Text = "";
            DisplayAlert("Cadastro",
                "Cadastro efetuado com sucesso!!!", "OK");
        }
        catch 
        {
            DisplayAlert("Cadastro", "Site já cadastrado!!!", "OK");
        }
        ListarSites();
    }

    private void alterarBtn_Clicked(object sender, EventArgs e)
    {
        Site site = new Site();
        if (int.TryParse(idEnt.Text, out int id))
        {
            site.Id = id;
        }
        else
        {
            DisplayAlert("Erro", "Informe apenas números para o código", "OK");
            return;
        }

        if (Uri.TryCreate(siteEnt.Text, UriKind.Absolute, out Uri urlResult))
        {
            site.Endereco = siteEnt.Text;
        }
        else
        {
            DisplayAlert("Erro", "Informe um endereço correto", "OK");
            return;
        }

        try
        {
            int qtd = conn.Update(site);
            idEnt.Text = "";
            siteEnt.Text = "";
            if (qtd > 0) DisplayAlert("Update","Registro atualizado com sucesso!!!", "OK");
            else DisplayAlert("Update", "O código não existe!!!!!", "OK");
            ListarSites();
        } catch
        {
            DisplayAlert("Erro","Site já cadastrado", "OK");
        }
    }

    private async void excluirBtn_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(idEnt.Text, out int id)== false)
        { 
            await DisplayAlert("Erro", "Informe apenas números para o código", "OK");
            return;
        }
        bool resp = await DisplayAlert("Excluir", "Deseja excluir?", "Sim","Não");
        if (resp)
        {
            conn.Delete<Site>(id);
            idEnt.Text = "";
            siteEnt.Text = "";
            await DisplayAlert("Delete","Registro deletado com sucesso!!!", "OK");
            ListarSites();
        }
    }

    private void localizarBtn_Clicked(object sender, EventArgs e)
    {
        string endereco = siteEnt.Text;

        var sites = from s in conn.Table<Site>()
                    where s.Endereco.Contains(endereco)
                    select s;

        if(sites.Count() <= 0)
        {
            ListarSites();
        }
        else
        {
            List<Site> lista = sites.ToList();
            ListaClv.ItemsSource = lista;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        if (label != null && label.BindingContext is Site item)
        {
            idEnt.Text = item.Id.ToString();
            siteEnt.Text = item.Endereco;
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        try 
        { 
            var label = sender as Label;
            if (label != null && label.BindingContext is Site item)
            {
                Launcher.OpenAsync(new Uri(item.Endereco));
            }
        }
        catch(Exception ex)
        {
            DisplayAlert("Erro", "Endereço incorreto", "OK");
        }
        
    }
}

