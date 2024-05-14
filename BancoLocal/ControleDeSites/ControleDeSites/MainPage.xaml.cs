using SQLite;

namespace ControleDeSites;

public partial class MainPage : ContentPage
{
	string dbPath; //indica onde esta o banco de dados
	SQLiteConnection conn; //representa a conexao com o banco de dados 

	public MainPage()
	{
		InitializeComponent();
	}

    public void ListarSites()
    {
        List<Site> lista = conn.Table<Site>().ToList();
        ListaClv.ItemsSource = lista;
    }

    private void criarBancoBtn_Clicked(object sender, EventArgs e)
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
        site.Endereco = siteEnt.Text;

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
        site.Id = Convert.ToInt32(idEnt.Text);
        site.Endereco = siteEnt.Text;

        conn.Update(site);
        idEnt.Text = "";
        siteEnt.Text = "";
        DisplayAlert("Update",
            "Registro atualizado com sucesso!!!", "OK");
        ListarSites();
    }

    private void excluirBtn_Clicked(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(idEnt.Text);
        conn.Delete<Site>(id);
        idEnt.Text = "";
        siteEnt.Text = "";
        DisplayAlert("Delete",
            "Registro deletado com sucesso!!!", "OK");
        ListarSites();
    }

    private void localizarBtn_Clicked(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(idEnt.Text);
        string endereco = siteEnt.Text;

        var sites = from s in conn.Table<Site>()
                    where s.Id == id
                    select s;

        //var sites = from s in conn.Table<Site>()
        //            where s.Endereco == endereco
        //            select s;

        Site site = sites.First();
        idEnt.Text = site.Id.ToString();
        siteEnt.Text = site.Endereco;
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
        var label = sender as Label;
        if (label != null && label.BindingContext is Site item)
        {
            Launcher.OpenAsync(new Uri(item.Endereco));
        }
    }
}

