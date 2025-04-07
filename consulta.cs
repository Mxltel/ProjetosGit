using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using MySql.Data.MySqlClient;

// Classe Empregado
public class Empregado
{
    // Propriedades
    public int Matricula { get; set; }
    public string Cpf { get; set; } = "";
    public string Nome { get; set; } = "";
    public string Endereco { get; set; } = "";

    // String de conexão (substitua pela sua senha se necessário)
    private string stringConexao = "server=localhost;user=root;password=;database=empresa";

    public DataTable Pesquisar()
    {
        StringBuilder stringSql = new StringBuilder();
        stringSql.Append("SELECT Matricula, CPF, Nome, Endereco ");
        stringSql.Append("FROM empregado ");
        stringSql.Append("WHERE 1 = 1");

        if (Matricula != 0)
            stringSql.Append(" AND Matricula = @matricula");

        if (!string.IsNullOrWhiteSpace(Cpf))
            stringSql.Append(" AND CPF = @cpf");

        if (!string.IsNullOrWhiteSpace(Nome))
            stringSql.Append(" AND Nome LIKE @nome");

        if (!string.IsNullOrWhiteSpace(Endereco))
            stringSql.Append(" AND Endereco LIKE @endereco");

        using (MySqlConnection conexao = new MySqlConnection(stringConexao))
        {
            MySqlCommand comando = new MySqlCommand(stringSql.ToString(), conexao);
            comando.Parameters.AddWithValue("@matricula", Matricula);
            comando.Parameters.AddWithValue("@cpf", Cpf);
            comando.Parameters.AddWithValue("@nome", $"%{Nome}%");
            comando.Parameters.AddWithValue("@endereco", $"%{Endereco}%");

            MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
            DataTable dados = new DataTable();
            adaptador.Fill(dados);
            return dados;
        }
    }
}

// Classe do Formulário
public class FormLocal : Form
{
    private TextBox textMatricula;
    private TextBox textCpf;
    private TextBox textNome;
    private TextBox textEndereco;
    private Button botaoPesquisar;
    private DataGridView dataGridViewEmpregado;

    public FormLocal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Pesquisa de Empregados";
        this.Size = new Size(700, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Layout
        TableLayoutPanel layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 5,
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(10)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

        // Matrícula
        layout.Controls.Add(new Label() { Text = "Matrícula:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
        textMatricula = new TextBox() { Anchor = AnchorStyles.Left | AnchorStyles.Right };
        layout.Controls.Add(textMatricula, 1, 0);

        // CPF
        layout.Controls.Add(new Label() { Text = "CPF:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
        textCpf = new TextBox() { Anchor = AnchorStyles.Left | AnchorStyles.Right };
        layout.Controls.Add(textCpf, 1, 1);

        // Nome
        layout.Controls
