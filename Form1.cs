using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TI_2._0
{
    public partial class Form1 : Form
    {
        Controle control;
        int contVoos;
        int contClientes;
        int contCidades;
        int contBilhetes;

        /// <summary>
        /// Inicializador do programa utilizando o forms
        /// </summary>
        public Form1()
        {
            control = new Controle();
            contVoos = -1;
            contClientes = -1;
            contCidades = -1;
            contBilhetes = -1;
            InitializeComponent();
            labelInfo.Text = "Seja bem vindo";
            labelInicialCidades.Text = "Nenhuma cidade cadastrada. Carregar arquivo ou adicionar nova cidade.";
            labelInicialVoos.Text = "Nenhum voo cadastrado. Carregar arquivo ou adicionar novo voo.";
            labelInicialClientes.Text = "Nenhum cliente cadastrado. Carregar arquivo ou adicionar novo cliente.";
            labelInicialBilhetes.Text = "Nenhum bilhete cadastrado. Carregar arquivo ou adicionar novo bilhete.";
        }

        /// <summary>
        /// Adiciona novo passageiro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AddPassageiro_Click(object sender, EventArgs e)
        {
            try
            {
                string aux1 = textBoxNomeCliente.Text;
                string aux2 = textBoxCidCliente.Text;

                if (aux1.Length == 0 || aux1.Equals("Nome") || aux1.Equals("Nome do cliente")) // Entrada errada para preenchimento do nome do novo cliente
                    throw new ExceptionError1();

                if (aux2.Length == 0 || aux2.Equals("Cidade") || aux2.Equals("Cidade de origem do cliente")) // Entrada errada para preenchimento do nome da cidade do novo cliente
                    throw new ExceptionError2();

                // Verifica se já existe cliente com o mesmo nome e mesma cidade 
                int aux = 0;
                for (int i = 0; i <= contClientes; i++)
                {
                    if ((aux1 == control.Clientes[i].Nome) && (aux2 == control.Clientes[i].CidadeOrigem))
                        aux++;
                }

                if (aux > 0) // Erro para cadastro de cliente repetido
                    throw new ExceptionGeral();

                // Verifica o código para o novo cliente
                // Código incrementado em 1 em relação ao maior código encontrado entre todos os clientes cadastrados
                double maior = 0;
                for (int i = 0; i <= contClientes; i++)
                {
                    if (control.Clientes[i].Cod > maior)
                        maior = control.Clientes[i].Cod;
                }

                double cod = maior + 1;
                contClientes++;
                Cliente novo = new Cliente(cod, aux1, aux2); // Cria novo cliente
                control.Clientes[contClientes] = novo; // Adiciona ao vetor de clientes do programa

                // Atualização e update de campos e listas
                textBoxNomeCliente.Text = "Nome";
                textBoxCidCliente.Text = "Cidade";
                labelInfo.Text = "Cliente " + control.Clientes[contClientes].Nome + " cadastrado com sucesso";
                uplistBoxClientesChanged();
                upListBoxClientes();
                upcomboBoxBilhetesBuscaNome();
                uplabelInicialClientes();
                textBoxNomeCliente.Focus();
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Preencher o campo NOME corretamente";
            }
            catch (ExceptionError2)
            {
                labelInfo.Text = "Preencher o campo CIDADE corretamente";
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Cliente já cadastrado";
            }
        }

        /// <summary>
        /// Buscar cliente por nome, sobrenome, cidade de origem ou código de cadastro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                if (contClientes == -1)
                    throw new ExceptionError2();

                string busca = textBoxBusca.Text;

                if (busca.Length == 0 || busca.Equals("Digite") || busca.Equals("Digite aqui o texto da busca"))
                    throw new ExceptionError1();

                string textBusca = busca.ToUpper();
                int aux = 0;

                listBoxClientes.Items.Clear();

                for (int i = 0; i <= contClientes; i++)
                {
                    string[] separa = control.Clientes[i].Nome.ToUpper().Split(' ');
                    int qtd = separa.Length;

                    // Verifica se o texto inserido pelo usuário é igual ao nome ou sobrenome de clientes cadastrados
                    for (int j = 0; j < qtd; j++)
                    {
                        if (separa[j].Equals(textBusca))
                        {
                            listBoxClientes.Items.Add(control.Clientes[i].Cod + ";" + control.Clientes[i].Nome + ";" + control.Clientes[i].CidadeOrigem);
                            aux++;
                        }
                    }
                }

                // Verifica se o texto inserido pelo usuário é igual ao nome da cidade de origem de clientes cadastrados
                for (int i = 0; i <= contClientes; i++)
                {
                    string[] separa = control.Clientes[i].CidadeOrigem.ToUpper().Split(' ');
                    int qtd = separa.Length;

                    for (int j = 0; j < qtd; j++)
                    {
                        if (separa[j].Equals(textBusca))
                        {
                            listBoxClientes.Items.Add(control.Clientes[i].Cod + ";" + control.Clientes[i].Nome + ";" + control.Clientes[i].CidadeOrigem);
                            aux++;
                        }
                    }
                }

                // Verifica se o texto inserido pelo usuário é igual ao código de cadastro de clientes cadastrados
                for (int i = 0; i <= contClientes; i++)
                {
                    double codBusca = control.Clientes[i].Cod;

                    for (int j = 0; j <= contClientes; j++)
                    {
                        if (codBusca.Equals(textBusca))
                        {
                            listBoxClientes.Items.Add(control.Clientes[i].Cod + ";" + control.Clientes[i].Nome + ";" + control.Clientes[i].CidadeOrigem);
                            aux++;
                        }
                    }
                }

                labelInfo.Text = aux + " clientes encontrados com a busca " + busca;

                // Mensagem caso o item buscado não tenha obtido retorno
                if (aux == 0)
                {
                    labelInfo.Text = "Nenhum cliente encontrado com a palavra " + busca;
                    textBoxBusca.Clear();
                    textBoxBusca.Focus();
                    upListBoxClientes();
                }
            }
            catch (ExceptionError2)
            {
                textBoxBusca.Text = "Digite aqui o texto da busca";
                labelInfo.Text = "Não há cliente cadastrado";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "O campo de busca está em branco";
            }

        }

        /// <summary>
        /// Excluir cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (contClientes == -1) // Se usuário tentar excluir sem nenhum cliente cadastrado
                    throw new ExceptionError1();

                int pos = listBoxClientes.SelectedIndex;
                string nome = control.Clientes[pos].Nome;

                if (pos > -1)
                {
                    control.Clientes[pos] = control.Clientes[contClientes];
                    contClientes--;
                    listBoxClientes.Items.Clear();
                }
                else // Se usuário tentar excluir sem selecionar nenhum cliente
                    throw new ExceptionError2();

                // Atualização e update de campos e listas
                labelInfo.Text = "Cliente " + nome + " excluído com sucesso";
                upListBoxClientes();
                listBoxVerCliente.Items.Clear();
                listBoxBilhetesCliente.Items.Clear();
                upcomboBoxBilhetesBuscaNome();
                uplabelInicialClientes();
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Não há cliente cadastrado";
            }
            catch (ExceptionError2)
            {
                labelInfo.Text = "Você deve selecionar um cliente na lista para exclui-lo";
            }
        }

        /// <summary>
        /// Aquisição de bilhete sem utilizar pontos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_VooComum_Click(object sender, EventArgs e)
        {
            int posCliente = listBoxClientes.SelectedIndex;
            int posVoo = comboBoxVoo.SelectedIndex;
            double pontos = 0;

            try
            {
                if (posVoo < 0)
                    throw new ExceptionError2();

                if (posCliente < 0) // Se usuário não selecionar cliente para adicionar passagem
                    throw new ExceptionError1();

                Bilhete bil;
                Data dt = new Data(control.Voos[posVoo].DataVoo.ToString());

                pontos = control.Voos[posVoo].Pontos * control.Clientes[posCliente].Categ.Bonificacao; // Pontos obtidos na aquisição do bilhete são multiplicados pelo bonus da categoria do cliente
                bil = new Bilhete(control.Clientes[posCliente].Cod, control.Voos[posVoo].Codigo, pontos, dt);

                contBilhetes++;
                control.Bilhetes[contBilhetes] = bil; // Adiciona novo bilhete ao vetor de bilhetes do programa

                control.Clientes[posCliente].addPontos(pontos); // Adiciona os pontos obtidos ao cliente

                // Atualização e update de campos e listas
                upListBoxVerCliente();
                uplabelInicialBilhetes();
                upListBoxBilhetesVendidos();
                uplistBoxBilhetesCliente(posCliente);
                labelInfo.Text = "Bilhete para voo " + bil.CodVoo + " cadastrado com sucesso";
            }
            catch (ExceptionError2)
            {
                labelInfo.Text = "Você deve selecionar um voo";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Você deve selecionar um cliente";
            }
        }

        /// <summary>
        /// Aquisição de bilhete utilizando pontos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPontos_Click(object sender, EventArgs e)
        {
            int posCliente = listBoxClientes.SelectedIndex;
            int posVoo = comboBoxVoo.SelectedIndex;
            int pontosMenos = 10000;

            try
            {
                if (posVoo < 0)
                    throw new ExceptionGeral();

                if (posCliente < 0) // Se usuário não selecionar cliente para adicionar passagem
                    throw new ExceptionError1();

                Bilhete bil;
                Data dt = new Data(control.Voos[posVoo].DataVoo.ToString());

                bil = new Bilhete(control.Clientes[posCliente].Cod, control.Voos[posVoo].Codigo, 0, dt);

                if (control.Clientes[posCliente].PontosAcumulados < pontosMenos) // Verifica se o cliente possui os pontos suficientes para aquisição do bilhete com pontos
                    throw new ExceptionError2();

                contBilhetes++;
                control.Bilhetes[contBilhetes] = bil; // Adiciona novo bilhete ao vetor de bilhetes do programa
                control.Clientes[posCliente].BilComPontos++; //Decrementa os pontos acumulados do cliente com a quantidade utilizada na aquisição do novo bilhete

                // Atualização e update de campos e listas
                upListBoxVerCliente();
                uplabelInicialBilhetes();
                upListBoxBilhetesVendidos();
                uplistBoxBilhetesCliente(posCliente);
                labelInfo.Text = "Bilhete para voo " + bil.CodVoo + " cadastrado com sucesso";
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Você deve selecionar um voo";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Você deve selecionar um cliente";
            }
            catch (ExceptionError2)
            {
                labelInfo.Text = "O cliente " + control.Clientes[posCliente].Nome + " não possui pontos suficientes";
            }
        }

        /// <summary>
        /// Sair do programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Informações sobre o programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelInfo.Text = ("TI.POO: Zamur, Madson, Mariano, Walison");
        }

        /// <summary>
        /// Ação de clique sobre campo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNomeCliente_Click(object sender, EventArgs e)
        {
            textBoxNomeCliente.Text = "";
        }

        /// <summary>
        /// Ação de clique sobre campo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCidCliente_Click(object sender, EventArgs e)
        {
            textBoxCidCliente.Text = "";
        }

        /// <summary>
        /// Ação de clique sobre campo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxBusca_Click(object sender, EventArgs e)
        {
            textBoxBusca.Text = "";
        }

        /// <summary>
        /// Ação de alteração de cliente selecionado na lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            uplistBoxClientesChanged();
        }

        private void uplistBoxClientesChanged()
        {
            listBoxVerCliente.Items.Clear();
            int pos = listBoxClientes.SelectedIndex;

            if (pos > -1)
            {
                uplistBoxBilhetesCliente(pos);
                labelInfo.Text = "Exibindo informações do cliente " + control.Clientes[pos].Nome;
                listBoxVerCliente.Items.Add("Código: " + control.Clientes[pos].Cod);
                listBoxVerCliente.Items.Add("Origem: " + control.Clientes[pos].CidadeOrigem);
                listBoxVerCliente.Items.Add("Categoria: " + control.Clientes[pos].Categ.Id);
                listBoxVerCliente.Items.Add("Total de pontos: " + control.Clientes[pos].PontosAcumulados);
                listBoxVerCliente.Items.Add("Pontos nos últimos 365 dias: " + control.Clientes[pos].Pontos12);
            }
        }

        /// <summary>
        /// Carregar arquivo contendo os dados de Cidade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.ShowDialog();
                string dados = openFileDialog.FileName;
                Cidade nova;

                if (!File.Exists(dados))
                    throw new ExceptionGeral(); // Gera erro caso o usuário abra um arquivo inexistente

                StreamReader leitura = new StreamReader(dados);
                string aux;

                while ((aux = leitura.ReadLine()) != null)
                {
                    string[] separa = aux.Split(';');

                    if (separa.Length != 1) // Gera erro caso o usuário abra arquivo contendo dados diferentes dos necessários para carregamento
                        throw new ExceptionError1();

                    nova = new Cidade(aux);
                    int aux2 = 0;

                    // Verifica cidade com nome repetido
                    for (int i = 0; i <= contCidades; i++)
                    {
                        if (nova.Id == control.Cidades[i].Id)
                            aux2++;
                    }

                    // Caso o nome da cidade não esteja se repetindo, nova cidade é inserida no vetor de cidades do programa
                    if (aux2 == 0)
                    {
                        contCidades++;
                        control.Cidades[contCidades] = nova;
                    }
                }
                leitura.Close();

                // Atualização e update de campos e listas
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " carregado com sucesso";
                uplistBoxCidadesCadastradas();
                upcomboBoxVooCidOrigem();
                upcomboBoxVooCidDestino();
                upcomboBoxBilhetesBuscaOrigem();
                upcomboBoxBilhetesBuscaDestino();
                uplabelInicialCidades();
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Arquivo não existente ou inválido";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "O arquivo " + openFileDialog.FileName.ToString() + " não está no formato correto";
            }
        }

        /// <summary>
        /// Carregar arquivo contendo os dados de Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.ShowDialog();
                string dados = openFileDialog.FileName;

                if (!File.Exists(dados))
                    throw new ExceptionGeral(); // Gera erro caso o usuário abra um arquivo inexistente

                StreamReader leitura = new StreamReader(dados);
                string aux;

                while ((aux = leitura.ReadLine()) != null)
                {
                    string[] separa = aux.Split(';');

                    if (separa.Length != 3)
                        throw new ExceptionError1();  // Gera erro caso o usuário abra arquivo contendo dados diferentes dos necessários para carregamento

                    Cliente cli = new Cliente(double.Parse(separa[0]), separa[1], separa[2]);

                    contClientes++;
                    control.Clientes[contClientes] = cli; // Novo cliente é inserido no vetor de clientes do programa
                }

                leitura.Close();

                // Atualização e update de campos e listas
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " carregado com sucesso";
                upListBoxClientes();
                upcomboBoxBilhetesBuscaNome();
                uplabelInicialClientes();
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Arquivo não existente ou inválido";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "O arquivo " + openFileDialog.FileName.ToString() + " não está no formato correto";
            }
        }

        /// <summary>
        /// Carregar arquivo contendo os dados de Voo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void voosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.ShowDialog();
                string dados = openFileDialog.FileName;

                if (!File.Exists(dados))
                    throw new ExceptionGeral(); // Gera erro caso o usuário abra um arquivo inexistente

                StreamReader leitura = new StreamReader(dados);
                string aux;

                comboBoxVoo.Items.Clear();

                while ((aux = leitura.ReadLine()) != null)
                {
                    string[] separa = aux.Split(';');
                    if (separa.Length != 5)
                        throw new ExceptionError1(); // Gera erro caso o usuário abra arquivo contendo dados diferentes dos necessários para carregamento

                    contVoos++;
                    Data nova = new Data(separa[3]);
                    Voo novo;

                    // Verifica modalidade do voo (Normal ou Promocional)
                    if (separa[4].Equals("Normal"))
                        novo = new VooNormal(Convert.ToInt32(separa[0]), separa[1], separa[2], nova);
                    else
                        novo = new VooPromocional(Convert.ToInt32(separa[0]), separa[1], separa[2], nova);

                    control.Voos[contVoos] = novo; // Adiciona o novo voo ao vetor de voos do programa

                    listBoxVoos.Items.Add(control.Voos[contVoos].Codigo + ";" + control.Voos[contVoos].DataVoo + ";" + control.Voos[contVoos].CidOrigem + ";" + control.Voos[contVoos].CidDestino + ";" + control.Voos[contVoos].Id);
                    comboBoxVoo.Items.Add(control.Voos[contVoos].Codigo + ";" + control.Voos[contVoos].DataVoo + ";" + control.Voos[contVoos].CidOrigem + ";" + control.Voos[contVoos].CidDestino + ";" + control.Voos[contVoos].Id);
                }
                leitura.Close();

                // Atualização e update de campos e listas
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " carregado com sucesso";
                uplabelInicialVoos();
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Arquivo não existente ou inválido";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "O arquivo " + openFileDialog.FileName.ToString() + " não está no formato correto";
            }
        }

        /// <summary>
        /// Ação do botão de listar todos os voos cadastrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonListarVoosCadastrados_Click(object sender, EventArgs e)
        {
            listBoxVoos.Items.Clear();
            for (int i = 0; i <= contVoos; i++)
                listBoxVoos.Items.Add(control.Voos[i].Codigo + ";" + control.Voos[i].CidOrigem + ";" + control.Voos[i].CidDestino + ";" + control.Voos[i].DataVoo);

            labelInfo.Text = "Listando todos os voos cadastrados";
        }

        private void bilhetesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.ShowDialog();
                string dados = openFileDialog.FileName;

                if (!File.Exists(dados))
                    throw new ExceptionGeral(); // Gera erro caso o usuário abra um arquivo inexistente

                StreamReader leitura = new StreamReader(dados);
                string aux;

                while ((aux = leitura.ReadLine()) != null)
                {
                    string[] separa = aux.Split(';');

                    if (separa.Length != 2)
                        throw new ExceptionError1();  // Gera erro caso o usuário abra arquivo contendo dados diferentes dos necessários para carregamento

                    double codCliente = double.Parse(separa[0]);
                    double codVoo = double.Parse(separa[1]);
                    Bilhete bil;

                    bil = new Bilhete(codCliente, codVoo);
                    contBilhetes++;
                    control.Bilhetes[contBilhetes] = bil; // Novo bilhete é inserido no vetor de bilhetes do programa
                }
                leitura.Close();

                // Atualização e update de campos e listas
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " carregado com sucesso";
                listBoxBilhetesVendidos.Items.Clear();
                uplabelInicialBilhetes();
                listBoxClientes.SelectedIndex = -1;
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Arquivo não existente ou inválido";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "O arquivo " + openFileDialog.FileName.ToString() + " não está no formato correto";
            }
        }

        /// <summary>
        /// Salvar arquivo contendo dados das cidades cadastradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cidadesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string arqDados = openFileDialog.FileName;

            if (File.Exists(arqDados))
            {
                StreamWriter dados = new StreamWriter(openFileDialog.FileName, false);

                for (int i = 0; i <= contCidades; i++)
                {
                    dados.WriteLine(control.Cidades[i].Id);
                }
                dados.Close();
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " salvo com sucesso";
            }
            else
            {
                saveFileDialog.ShowDialog();

                StreamWriter dados = new StreamWriter(saveFileDialog.FileName + ".txt");

                for (int i = 0; i <= contCidades; i++)
                {
                    dados.WriteLine(control.Cidades[i].Id);
                }
                dados.Close();
                labelInfo.Text = "Arquivo " + saveFileDialog.FileName + " salvo com sucesso";
            }
        }

        /// <summary>
        /// Salvar arquivo contendo dados dos clientes cadastrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clientesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string arqDados = openFileDialog.FileName;

            if (File.Exists(arqDados))
            {
                StreamWriter dados = new StreamWriter(openFileDialog.FileName, false);

                for (int i = 0; i <= contClientes; i++)
                {
                    dados.WriteLine(control.Clientes[i].Cod + ";" + control.Clientes[i].Nome + ";" + control.Clientes[i].CidadeOrigem);
                }
                dados.Close();
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " salvo com sucesso";
            }
            else
            {
                saveFileDialog.ShowDialog();

                StreamWriter dados = new StreamWriter(saveFileDialog.FileName + ".txt");

                for (int i = 0; i <= contClientes; i++)
                {
                    dados.WriteLine(control.Clientes[i].Cod + ";" + control.Clientes[i].Nome + ";" + control.Clientes[i].CidadeOrigem);
                }
                dados.Close();
                labelInfo.Text = "Arquivo " + saveFileDialog.FileName + " salvo com sucesso";
            }
        }

        /// <summary>
        /// Ação para o botão de excluir cidade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCidadeExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = listBoxCidadesCadastradas.SelectedIndex;

                if (contCidades == -1)
                    throw new ExceptionError1(); // Gera erro caso não haja nenhuma cidade cadastrada

                if (pos > -1)
                {
                    string cid = control.Cidades[pos].Id;
                    control.Cidades[pos] = control.Cidades[contCidades];
                    contCidades--;

                    // Atualização e update de campos e listas
                    listBoxCidadesCadastradas.Items.Clear();
                    labelInfo.Text = "Cidade " + cid + " excluída com sucesso";
                    uplistBoxCidadesCadastradas();
                    upcomboBoxBilhetesBuscaOrigem();
                    upcomboBoxBilhetesBuscaDestino();
                    uplabelInicialCidades();
                }
                else
                    throw new ExceptionError2(); // Gera erro caso nenhuma cidade tenha sido selecionada
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Não há cidade cadastrada";
            }
            catch (ExceptionError2)
            {
                labelInfo.Text = "Você deve selecionar a cidade na lista para exclui-la";
            }
        }

        /// <summary>
        /// Ação para o botão de excluir voo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVooExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = listBoxVoos.SelectedIndex;

                if (contVoos == -1)
                    throw new ExceptionGeral(); // Gera erro caso não haja nenhum voo cadastrado

                if (pos > -1)
                {
                    double cod = control.Voos[pos].Codigo;
                    control.Voos[pos] = control.Voos[contVoos];
                    contVoos--;

                    // Atualização e update de campos e listas
                    listBoxVoos.Items.Clear();
                    labelInfo.Text = "Voo " + cod + " excluído com sucesso";
                    uplistBoxVoos();
                    upComboBoxVoo();
                    uplabelInicialVoos();
                }
                else
                    throw new ArgumentNullException(); // Gera erro caso nenhum voo tenha sido selecionado
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Não há voo cadastrado";
            }
            catch (ArgumentNullException)
            {
                labelInfo.Text = "Você deve selecionar um voo na lista para exclui-lo";
            }
        }

        /// <summary>
        /// Ação para botão de cadastrar nova cidade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCadastrarCidade_Click(object sender, EventArgs e)
        {
            string cd = textBoxCidadeNova.Text;

            try
            {
                if (cd.Length == 0 || cd.Equals("Cidade"))
                    throw new ArgumentNullException(); // Gera erro caso o campo com nome da nova cidade esteja vazio

                // Verifica se já existe cidade com o nome da nova cidade
                int aux = 0;
                for (int i = 0; i <= contCidades; i++)
                {
                    if (cd == control.Cidades[i].Id)
                        aux++;
                }

                if (aux != 0)
                    throw new ExceptionGeral(); // Gera erro caso já exista cidade com o mesmo nome cadastrada

                contCidades++;
                Cidade nova = new Cidade(cd);
                control.Cidades[contCidades] = nova; // Insere nova cidade no vetor Cidades do programa

                // Atualização e update de campos e listas
                labelInfo.Text = "Cidade cadastrada com sucesso";
                comboBoxVooCidOrigem.Items.Add(control.Cidades[contCidades].Id);
                comboBoxVooCidDestino.Items.Add(control.Cidades[contCidades].Id);
                textBoxCidadeNova.Clear();
                textBoxCidadeNova.Focus();
                upcomboBoxBilhetesBuscaOrigem();
                upcomboBoxBilhetesBuscaDestino();
                uplistBoxCidadesCadastradas();
                uplabelInicialCidades();
            }
            catch (ArgumentNullException)
            {
                textBoxCidadeNova.Text = "Cidade";
                labelInfo.Text = "Campo de preenchimento de nova cidade está vazio";
            }

            catch (ExceptionGeral)
            {
                textBoxCidadeNova.Text = cd + " já está cadastrada";
                labelInfo.Text = "Cidade já cadastrada";
            }
        }

        /// <summary>
        /// Ação ao entrar no campo de preenchimento do botão
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCidadeNova_Enter(object sender, EventArgs e)
        {
            textBoxCidadeNova.Clear();
        }

        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        private void uplistBoxCidadesCadastradas()
        {
            listBoxCidadesCadastradas.Items.Clear();

            for (int i = 0; i <= contCidades; i++)
            {
                listBoxCidadesCadastradas.Items.Add(control.Cidades[i].Id);
            }
        }

        /// <summary>
        /// Update da comboBox com novas informações registradas
        /// </summary>
        private void upcomboBoxBilhetesBuscaNome()
        {
            comboBoxBilhetesBuscaNome.Items.Clear();
            for (int i = 0; i <= contClientes; i++)
            {
                comboBoxBilhetesBuscaNome.Items.Add(control.Clientes[i].Nome);
            }
        }

        /// <summary>
        /// Update da comboBox com novas informações registradas
        /// </summary>
        private void upcomboBoxBilhetesBuscaOrigem()
        {
            comboBoxBilhetesBuscaOrigem.Items.Clear();

            for (int i = 0; i <= contCidades; i++)
            {
                comboBoxBilhetesBuscaOrigem.Items.Add(control.Cidades[i].Id);
            }
        }

        /// <summary>
        /// Update da comboBox com novas informações registradas
        /// </summary>
        private void upcomboBoxBilhetesBuscaDestino()
        {
            comboBoxBilhetesBuscaDestino.Items.Clear();

            for (int i = 0; i <= contCidades; i++)
            {
                comboBoxBilhetesBuscaDestino.Items.Add(control.Cidades[i].Id);
            }
        }

        /// <summary>
        /// Update da comboBox com novas informações registradas
        /// </summary>
        private void upcomboBoxVooCidOrigem()
        {
            comboBoxVooCidOrigem.Items.Clear();
            for (int i = 0; i <= contCidades; i++)
                comboBoxVooCidOrigem.Items.Add(control.Cidades[i].Id);
        }

        /// <summary>
        /// Update da comboBox com novas informações registradas
        /// </summary>
        private void upcomboBoxVooCidDestino()
        {
            comboBoxVooCidDestino.Items.Clear();
            for (int i = 0; i <= contCidades; i++)
                comboBoxVooCidDestino.Items.Add(control.Cidades[i].Id);
        }

        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        private void upListBoxVerCliente()
        {
            listBoxVerCliente.Items.Clear();
            int pos = listBoxClientes.SelectedIndex;

            if (pos > -1)
            {
                uplistBoxBilhetesCliente(pos);
                labelInfo.Text = "Exibindo informações do cliente " + control.Clientes[pos].Nome;
                listBoxVerCliente.Items.Add("Código: " + control.Clientes[pos].Cod);
                listBoxVerCliente.Items.Add("Origem: " + control.Clientes[pos].CidadeOrigem);
                listBoxVerCliente.Items.Add("Categoria: " + control.Clientes[pos].Categ.Id);
                listBoxVerCliente.Items.Add("Total de pontos: " + control.Clientes[pos].PontosAcumulados);
                listBoxVerCliente.Items.Add("Pontos nos últimos 365 dias: " + control.Clientes[pos].Pontos12);
            }

        }

        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        private void uplistBoxVoos()
        {
            listBoxVoos.Items.Clear();
            for (int i = 0; i <= contVoos; i++)
                listBoxVoos.Items.Add("Código: " + control.Voos[i].Codigo + ";" + control.Voos[i].DataVoo + ";" + control.Voos[i].CidOrigem + ";" + control.Voos[i].CidDestino + ";" + control.Voos[i].Id);
        }

        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        /// <param name="pos"></param>
        private void uplistBoxBilhetesCliente(int pos)
        {
            listBoxBilhetesCliente.Items.Clear();

            double codCliente = control.Clientes[pos].Cod;
            double codVoo = 0;
            Data dt = new Data("01/01/2000");
            double pts = 0;

            control.Clientes[pos].PontosAcumulados = 0;
            control.Clientes[pos].Pontos12 = 0;
            int aux = 0;

            for (int i = 0; i <= contBilhetes; i++)
            {
                if (control.Bilhetes[i].CodCliente == codCliente)
                {
                    codVoo = control.Bilhetes[i].CodVoo;
                    for (int j = 0; j <= contVoos; j++)
                    {
                        if (control.Voos[j].Codigo == codVoo)
                        {
                            dt = new Data(control.Voos[j].DataVoo.ToString());
                            pts = control.Clientes[pos].Categ.Bonificacao * control.Voos[j].Pontos;

                            control.Clientes[pos].addPontos(pts);
                            control.Clientes[pos].verifPontos12(dt, pts);

                            listBoxBilhetesCliente.Items.Add("Bilhete do voo " + control.Bilhetes[i].CodVoo);
                            listBoxBilhetesCliente.Items.Add("Data: " + control.Voos[j].DataVoo);
                            listBoxBilhetesCliente.Items.Add("Tipo: " + control.Voos[j].Id);
                            listBoxBilhetesCliente.Items.Add("Origem: " + control.Voos[j].CidOrigem);
                            listBoxBilhetesCliente.Items.Add("Destino: " + control.Voos[j].CidDestino);
                            listBoxBilhetesCliente.Items.Add("Pontos gerados: " + control.Voos[j].Pontos);
                            listBoxBilhetesCliente.Items.Add("___________________");

                            aux++;
                        }
                    }
                }
            }

            int aux2 = 0;
            if (control.Clientes[pos].BilComPontos > 0)
            {
                aux2 = control.Clientes[pos].BilComPontos;
                for (int i = 0; i < aux2; i++)
                {
                    control.Clientes[pos].subPontos(10000);
                }
            }

            if (aux == 0)
                listBoxBilhetesCliente.Items.Add("Cliente " + control.Clientes[pos].Nome + " ainda não possui bilhetes");
        }

        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        /// <param name="pos"></param>
        private void uplistBoxBilhetesVendidosPorNome()
        {
            listBoxBilhetesVendidos.Items.Clear();

            int pos = comboBoxBilhetesBuscaNome.SelectedIndex;

            double codCliente = control.Clientes[pos].Cod;
            double codVoo = 0;
            Data dt = new Data("01/01/2000");
            double pts = 0;

            control.Clientes[pos].PontosAcumulados = 0;
            control.Clientes[pos].Pontos12 = 0;
            int aux = 0;

            for (int i = 0; i <= contBilhetes; i++)
            {
                if (control.Bilhetes[i].CodCliente == codCliente)
                {
                    codVoo = control.Bilhetes[i].CodVoo;
                    for (int j = 0; j <= contVoos; j++)
                    {
                        if (control.Voos[j].Codigo == codVoo)
                        {
                            dt = new Data(control.Voos[j].DataVoo.ToString());
                            pts = control.Clientes[pos].Categ.Bonificacao * control.Voos[j].Pontos;

                            control.Clientes[pos].addPontos(pts);
                            control.Clientes[pos].verifPontos12(dt, pts);
                            listBoxBilhetesVendidos.Items.Add("Código: " + control.Bilhetes[i].CodVoo + ";" + control.Voos[j].DataVoo + ";" + control.Voos[j].CidOrigem + ";" + control.Voos[j].CidDestino);
                            aux++;
                        }
                    }
                }
            }

            int aux2 = 0;
            if (control.Clientes[pos].BilComPontos > 0)
            {
                aux2 = control.Clientes[pos].BilComPontos;
                for (int i = 0; i < aux2; i++)
                {
                    control.Clientes[pos].subPontos(10000);
                }
            }

            if (aux == 0)
                listBoxBilhetesVendidos.Items.Add("Cliente " + control.Clientes[pos].Nome + " ainda não possui bilhetes");
        }


        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        /// <param name="pos"></param>
        private void uplistBoxBilhetesVendidosPorOrigem()
        {
            listBoxBilhetesVendidos.Items.Clear();

            int pos = comboBoxBilhetesBuscaOrigem.SelectedIndex;
            string cid = control.Cidades[pos].Id;
            int aux = 0;
            double codVoo = 0;

            for (int j = 0; j <= contVoos; j++)
            {
                if (cid == control.Voos[j].CidOrigem)
                {
                    codVoo = control.Voos[j].Codigo;

                    for (int i = 0; i <= contBilhetes; i++)
                    {
                        if (control.Bilhetes[i].CodVoo == codVoo)
                        {
                            listBoxBilhetesVendidos.Items.Add("Código: " + control.Bilhetes[i].CodVoo + ";" + control.Voos[j].DataVoo + ";" + control.Voos[j].CidOrigem + ";" + control.Voos[j].CidDestino);
                            aux++;
                        }
                    }
                }
            }

            if (aux == 0)
                listBoxBilhetesVendidos.Items.Add("Cidade " + control.Cidades[pos].Id + " ainda não possui bilhetes");
        }



        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        /// <param name="pos"></param>
        private void uplistBoxBilhetesVendidosPorDestino()
        {
            listBoxBilhetesVendidos.Items.Clear();

            int pos = comboBoxBilhetesBuscaDestino.SelectedIndex;
            string cid = control.Cidades[pos].Id;
            int aux = 0;
            double codVoo = 0;

            for (int j = 0; j <= contVoos; j++)
            {
                if (cid == control.Voos[j].CidOrigem)
                {
                    codVoo = control.Voos[j].Codigo;

                    for (int i = 0; i <= contBilhetes; i++)
                    {
                        if (control.Bilhetes[i].CodVoo == codVoo)
                        {
                            listBoxBilhetesVendidos.Items.Add("Código: " + control.Bilhetes[i].CodVoo + ";" + control.Voos[j].DataVoo + ";" + control.Voos[j].CidOrigem + ";" + control.Voos[j].CidDestino);
                            aux++;
                        }
                    }
                }
            }

            if (aux == 0)
                listBoxBilhetesVendidos.Items.Add("Cidade " + control.Cidades[pos].Id + " ainda não possui bilhetes");
        }


        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        private void upListBoxClientes()
        {
            listBoxClientes.Items.Clear();
            for (int i = 0; i <= contClientes; i++)
            {
                listBoxClientes.Items.Add(control.Clientes[i].Nome);
                control.Clientes[i].setCategoria();
            }
        }

        /// <summary>
        /// Update da lista com novas informações registradas
        /// </summary>
        private void upListBoxBilhetesVendidos()
        {
            listBoxBilhetesVendidos.Items.Clear();

            for (int i = 0; i <= contBilhetes; i++)
            {
                listBoxBilhetesVendidos.Items.Add("Cliente: " + control.Bilhetes[i].CodCliente + "; Voo: " + control.Bilhetes[i].CodVoo);
            }
        }

        /// <summary>
        /// Update da comboBox com novas informações registradas
        /// </summary>
        private void upComboBoxVoo()
        {
            comboBoxVoo.Items.Clear();
            for (int i = 0; i <= contVoos; i++)
                comboBoxVoo.Items.Add(control.Voos[i].Codigo + ";" + control.Voos[i].DataVoo + ";" + control.Voos[i].CidOrigem + ";" + control.Voos[i].CidDestino + ";" + control.Voos[i].Id);
        }

        /// <summary>
        /// Update da label com novas informações registradas
        /// </summary>
        private void uplabelInicialCidades()
        {
            if (contCidades == 0)
                labelInicialCidades.Text = (contCidades + 1) + " cidade cadastrada.";
            else
                labelInicialCidades.Text = (contCidades + 1) + " cidades cadastradas.";
        }

        /// <summary>
        /// Update da label com novas informações registradas
        /// </summary>
        private void uplabelInicialVoos()
        {
            if (contVoos == 0)
                labelInicialVoos.Text = (contVoos + 1) + " voo cadastrado.";
            else
                labelInicialVoos.Text = (contVoos + 1) + " voos cadastrados.";
        }

        /// <summary>
        /// Update da label com novas informações registradas
        /// </summary>
        private void uplabelInicialClientes()
        {
            if (contClientes == 0)
                labelInicialClientes.Text = (contClientes + 1) + " cliente cadastrado.";
            else
                labelInicialClientes.Text = (contClientes + 1) + " clientes cadastrados.";
        }

        /// <summary>
        /// Update da label com novas informações registradas
        /// </summary>
        private void uplabelInicialBilhetes()
        {
            if (contBilhetes == 0)
                labelInicialBilhetes.Text = (contBilhetes + 1) + " bilhete cadastrado.";
            else
                labelInicialBilhetes.Text = (contBilhetes + 1) + " bilhetes cadastrados.";
        }

        private void upDataBilhetes()
        {
            if ((contVoos > -1) && (contBilhetes > -1))
            {
                Data dt = new Data("01/01/2000");
                double codVoo = 0;

                for (int i = 0; i <= contVoos; i++)
                {
                    dt = control.Voos[i].DataVoo;
                    codVoo = control.Voos[i].Codigo;

                    for (int j = 0; j <= contBilhetes; j++)
                    {
                        if (codVoo == control.Bilhetes[j].CodVoo)
                        {
                            control.Bilhetes[j].Dt = dt;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ação do botão cadastrar novo voo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCadastrarNovoVoo_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica o código que o novo voo irá receber
                int maior = 0;
                for (int i = 0; i <= contVoos; i++)
                {
                    if (control.Voos[i].Codigo > maior)
                        maior = control.Voos[i].Codigo;
                }

                int cod = maior + 1;
                Data nova = new Data(dateTimePickerVoo.Text);
                string cidOr = comboBoxVooCidOrigem.Text;
                string cidDes = comboBoxVooCidDestino.Text;

                // Verifica se alguma cidade foi selecionada na comboBox de cidade de origem do voo
                int aux = 0;
                for (int i = 0; i <= contCidades; i++)
                {
                    if (cidOr == control.Cidades[i].Id)
                        aux++;
                }

                if (aux == 0)
                    throw new ExceptionError1(); // Gera erro caso nenhuma cidade de origem tenha sido selecionada

                // Verifica se alguma cidade foi selecionada na comboBox de cidade de destino do voo
                int aux2 = 0;
                for (int i = 0; i <= contCidades; i++)
                {
                    if (cidDes == control.Cidades[i].Id)
                        aux2++;
                }

                if (aux2 == 0)
                    throw new ExceptionError2(); // Gera erro caso nenhuma cidade de destino tenha sido selecionada

                if (cidDes == cidOr)
                    throw new ExceptionGeral(); // Gera erro caso a cidade de origem e a cidade de destino sejam iguais

                Voo novo;

                // Verifica tipo de voo (Normal ou Promocional)
                if (radioButtonNormal.Checked == true)
                    novo = new VooNormal(cod, cidOr, cidDes, nova);
                else
                    novo = new VooPromocional(cod, cidOr, cidDes, nova);

                contVoos++;
                control.Voos[contVoos] = novo; // Insere novo voo no vetor de voos do programa

                // Atualização e update de campos e listas
                labelInfo.Text = "Voo " + cod + " cadastrado com sucesso";
                upcomboBoxVooCidDestino();
                comboBoxVooCidDestino.Text = "Selecionar cidade de destino";
                upcomboBoxVooCidOrigem();
                comboBoxVooCidOrigem.Text = "Selecionar cidade de origem";
                uplabelInicialVoos();
                uplistBoxVoos();
                upComboBoxVoo();
            }

            catch (ExceptionError1)
            {
                labelInfo.Text = "Selecionar a cidade de origem";
            }
            catch (ExceptionError2)
            {
                labelInfo.Text = "Selecionar a cidade de destino";
            }
            catch (ExceptionGeral)
            {
                labelInfo.Text = "Cidade de destino e de origem iguais. Alterar uma das duas";
            }
        }

        /// <summary>
        /// Salvar arquivo contendo dados dos voos cadastrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void voosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string arqDados = openFileDialog.FileName;

            if (File.Exists(arqDados))
            {
                StreamWriter dados = new StreamWriter(openFileDialog.FileName, false);

                for (int i = 0; i <= contVoos; i++)
                {
                    dados.WriteLine(control.Voos[i].Codigo + ";" + control.Voos[i].CidOrigem + ";" + control.Voos[i].CidDestino + ";" + control.Voos[i].DataVoo + ";" + control.Voos[i].Id);
                }
                dados.Close();
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " salvo com sucesso";
            }
            else
            {
                saveFileDialog.ShowDialog();

                StreamWriter dados = new StreamWriter(saveFileDialog.FileName + ".txt");

                for (int i = 0; i <= contVoos; i++)
                {
                    dados.WriteLine(control.Voos[i].Codigo + ";" + control.Voos[i].CidOrigem + ";" + control.Voos[i].CidDestino + ";" + control.Voos[i].DataVoo + ";" + control.Voos[i].Id);
                }
                dados.Close();
                labelInfo.Text = "Arquivo " + saveFileDialog.FileName.ToString() + " salvo com sucesso";
            }
        }

        /// <summary>
        /// Salvar arquivo contendo dados dos voos cadastrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bilhetesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string arqDados = openFileDialog.FileName;

            if (File.Exists(arqDados))
            {
                StreamWriter dados = new StreamWriter(openFileDialog.FileName, false);

                for (int i = 0; i <= contBilhetes; i++)
                {
                    dados.WriteLine(control.Bilhetes[i].CodCliente + ";" + control.Bilhetes[i].CodVoo);
                }

                dados.Close();
                labelInfo.Text = "Arquivo " + openFileDialog.FileName.ToString() + " salvo com sucesso";
            }
            else
            {
                saveFileDialog.ShowDialog();

                StreamWriter dados = new StreamWriter(saveFileDialog.FileName + ".txt");

                for (int i = 0; i <= contBilhetes; i++)
                {
                    dados.WriteLine(control.Bilhetes[i].CodCliente + ";" + control.Bilhetes[i].CodVoo);
                }

                dados.Close();
                labelInfo.Text = "Arquivo " + saveFileDialog.FileName.ToString() + " salvo com sucesso";
            }
        }

        /// <summary>
        /// Ação do botão de listar todos os bilhetes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBilhetesListarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                if (contBilhetes < 0)
                    throw new ExceptionError1();
                labelInfo.Text = "Listando todos os bilhetes emitidos";
                upListBoxBilhetesVendidos();
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Nenhum bilhete cadastrado";
            }
        }

        /// <summary>
        /// Ação do botão de listar todos os voos de acordo com a cidade de origem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonListarPorOrigem_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxBilhetesVendidos.Items.Clear();
                int pos = comboBoxBilhetesBuscaOrigem.SelectedIndex;

                if ((pos == -1) || comboBoxBilhetesBuscaNome.Text.Equals("Selecione a cidade de origem"))
                    throw new ExceptionError1();

                uplistBoxBilhetesVendidosPorOrigem();

                comboBoxBilhetesBuscaOrigem.Text = "Selecione a cidade de origem";
                labelInfo.Text = "Listando bilhetes emitidos com origem em " + control.Cidades[pos].Id;
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Escolher nome para busca";
            }
        }

        /// <summary>
        /// Ação do botão de listar todos os bilhetes de acordo com a cidade de destino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonListarPorDestino_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxBilhetesVendidos.Items.Clear();
                int pos = comboBoxBilhetesBuscaDestino.SelectedIndex;

                if ((pos == -1) || comboBoxBilhetesBuscaNome.Text.Equals("Selecione a cidade de destino"))
                    throw new ExceptionError1();

                uplistBoxBilhetesVendidosPorDestino();

                comboBoxBilhetesBuscaDestino.Text = "Selecione a cidade de destino";
                labelInfo.Text = "Listando bilhetes emitidos com destino a " + control.Cidades[pos].Id;
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Escolher nome para busca";
            }
        }

        /// <summary>
        /// Ação do botão de listar todos os bilhetes de acordo com a data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonListarPorData_Click(object sender, EventArgs e)
        {
            listBoxBilhetesVendidos.Items.Clear();
            Data nova = new Data(dateTimePickerBilhetesBuscaData.Text);

            for (int j = 0; j <= contVoos; j++)
            {
                if (control.Voos[j].DataVoo.Equals(nova))
                {
                    for (int i = 0; i <= contBilhetes; i++)
                    {
                        if (control.Voos[j].Codigo == control.Bilhetes[i].CodVoo)
                            listBoxBilhetesVendidos.Items.Add("Código voo: " + control.Bilhetes[i].CodVoo + "; Código bilhete: " + control.Bilhetes[i].CodCliente);
                    }
                }
            }

            labelInfo.Text = "Listando bilhetes emitidos em " + nova;
        }

        /// <summary>
        /// Ação do botão de listar todos os bilhetes de acordo com o nome do cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonListarPorNome_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxBilhetesVendidos.Items.Clear();
                int pos = comboBoxBilhetesBuscaNome.SelectedIndex;
                if ((pos == -1) || comboBoxBilhetesBuscaNome.Text.Equals("Selecione um cliente pelo nome"))
                    throw new ExceptionError1();

                double codCliente = control.Clientes[pos].Cod;

                uplistBoxBilhetesVendidosPorNome();

                comboBoxBilhetesBuscaNome.Text = "Selecione um cliente pelo nome";
                labelInfo.Text = "Listando bilhetes emitidos para o cliente " + control.Clientes[pos].Nome;
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Escolher nome para busca";
            }

        }

        /// <summary>
        /// Ação do botão de listar todos os bilhetes de acordo período entre duas datas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBilhetesBusca_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxBilhetesVendidos.Items.Clear();

                Data inicial = new Data(dateTimePickerBilhetesInicial.Text);
                inicial.AcrescentaDias(-1);
                int[] dtIni = inicial.separaDados();
                DateTime ini = new DateTime(dtIni[0], dtIni[1], dtIni[2]);

                Data final = new Data(dateTimePickerBilhetesFinal.Text);
                final.AcrescentaDias(1);
                int[] dtFin = final.separaDados();
                DateTime fin = new DateTime(dtFin[0], dtFin[1], dtFin[2]);

                if (ini.CompareTo(fin) == 1)
                    throw new ExceptionError1(); // Gera erro caso a data inicial seja mais recente que a data final

                int[] dtBilhete;
                DateTime meio;

                for (int j = 0; j <= contVoos; j++)
                {
                    dtBilhete = control.Voos[j].DataVoo.separaDados();
                    meio = new DateTime(dtBilhete[0], dtBilhete[1], dtBilhete[2]);

                    if ((meio > ini) && (meio < fin))
                    {
                        for (int i = 0; i <= contBilhetes; i++)
                        {
                            if (control.Voos[j].Codigo == control.Bilhetes[i].CodVoo)
                                listBoxBilhetesVendidos.Items.Add("Código do voo: " + control.Bilhetes[i].CodVoo + "; Código do cliente: " + control.Bilhetes[i].CodCliente);
                        }
                    }
                }
                labelInfo.Text = "Listando bilhetes emitidos entre " + dateTimePickerBilhetesInicial.Text + " e " + dateTimePickerBilhetesFinal.Text;
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "A data inicial deve ser anterior à data final";
            }
        }

        /// <summary>
        /// Ação do botão de listar todos os clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonListarTodosClientes_Click(object sender, EventArgs e)
        {
            try
            {
                if (contClientes == -1)
                    throw new ExceptionError1();
                else
                    upListBoxClientes();
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Nenhum cliente cadastrado";
            }
        }

        /// <summary>
        /// Ação de seleção de voo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxVoo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (contVoos == -1)
                labelInfo.Text = "Não há voo cadastrado";
        }

        private void tabControlGeral_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelInfo.Text = "";
        }

        private void buttonListarTodosVoos_Click(object sender, EventArgs e)
        {
            try
            {
                if (contVoos < 0)
                    throw new ExceptionError1();

                uplistBoxVoos();
                labelInfo.Text = "Listando todos os voos";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Não há voo cadastrado";
            }
        }

        /// <summary>
        /// Ação para botão excluir bilhete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExcluirBilhete_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = listBoxBilhetesVendidos.SelectedIndex;

                if (pos < 0)
                    throw new ExceptionError1();

                double codCliente = control.Bilhetes[pos].CodCliente;
                double codVoo = control.Bilhetes[pos].CodVoo;

                double pts = control.Bilhetes[pos].PontosGerados;

                for (int i = 0; i <= contBilhetes; i++)
                {
                    if (codCliente == control.Bilhetes[i].CodCliente)
                        if (codVoo == control.Bilhetes[i].CodVoo)
                        {
                            if (pts == control.Bilhetes[i].PontosGerados)
                            {
                                control.Bilhetes[i] = control.Bilhetes[contBilhetes];
                                contBilhetes--;
                            }
                        }
                }
                labelInfo.Text = "Bilhete excluído com sucesso";
                upListBoxBilhetesVendidos();

            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Você deve selecionar um bilhete para exclui-lo";
            }
        }

        private void buttonListarTodasCidades_Click(object sender, EventArgs e)
        {
            try
            {
                if (contCidades < 0)
                    throw new ExceptionError1();
                labelInfo.Text = "Listando todas as cidades cadastradas";
                uplistBoxCidadesCadastradas();
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "Não há cidade cadastrada";
            }
        }

        private void buttonCidade_Click(object sender, EventArgs e)
        {
            try
            {
                if (contCidades == -1)
                    throw new ExceptionError2();

                string busca = textBoxBuscaCidade.Text;

                if (busca.Length == 0 || busca.Equals("Digite") || busca.Equals("Digite aqui o texto da busca"))
                    throw new ExceptionError1();

                string textBusca = busca.ToUpper();
                int aux = 0;

                listBoxCidadesCadastradas.Items.Clear();

                for (int i = 0; i <= contCidades; i++)
                {
                    string[] separa = control.Cidades[i].Id.ToUpper().Split(' ');
                    int qtd = separa.Length;

                    // Verifica se o texto inserido pelo usuário é igual ao nome de cidades cadastradas
                    for (int j = 0; j < qtd; j++)
                    {
                        if (separa[j].Equals(textBusca))
                        {
                            listBoxCidadesCadastradas.Items.Add(control.Cidades[i].Id);
                            aux++;
                        }
                    }
                }

                labelInfo.Text = aux + " cidades encontradas com a busca " + busca;
                textBoxBuscaCidade.Clear();

                // Mensagem caso o item buscado não tenha obtido retorno
                if (aux == 0)
                {
                    labelInfo.Text = "Nenhuma cidade encontrada com a palavra " + busca;
                    textBoxBuscaCidade.Text = "Digite aqui o texto da busca";
                    textBoxBuscaCidade.Focus();
                    uplistBoxCidadesCadastradas();
                }
            }
            catch (ExceptionError2)
            {
                textBoxBusca.Text = "Digite aqui o texto da busca";
                labelInfo.Text = "Não há cidade cadastrada";
            }
            catch (ExceptionError1)
            {
                labelInfo.Text = "O campo de busca está em branco";
            }
        }

        private void textBoxBuscaCidade_Click(object sender, EventArgs e)
        {
            textBoxBuscaCidade.Text = "";
        }

        private void buttonBuscarClienteBilDatas_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxBilhetesCliente.Items.Clear();

                Data inicial = new Data(dateTimePickerBilClienteIni.Text);
                int[] dtIni = inicial.separaDados();
                DateTime ini = new DateTime(dtIni[0], dtIni[1], dtIni[2]);

                Data final = new Data(dateTimePickerBilClienteFin.Text);
                int[] dtFin = final.separaDados();
                DateTime fin = new DateTime(dtFin[0], dtFin[1], dtFin[2]);

                if (fin < ini)
                    throw new ExceptionError1(); // Gera erro caso a data inicial seja mais recente que a data final

                Data dataMeio = new Data("01/01/2000");
                int[] dtBilhete;
                DateTime meio;

                int pos = listBoxClientes.SelectedIndex;
                if (pos < 0)
                    throw new ExceptionError1();

                double codCliente = control.Clientes[pos].Cod;
                double codVoo = 0;

                int aux = 0;

                for (int i = 0; i <= contBilhetes; i++)
                {
                    if (control.Bilhetes[i].CodCliente == codCliente)
                    {
                        codVoo = control.Bilhetes[i].CodVoo;
                        for (int j = 0; j <= contVoos; j++)
                        {
                            if (control.Voos[j].Codigo == codVoo)
                            {
                                dataMeio = control.Voos[j].DataVoo;
                                dtBilhete = dataMeio.separaDados();
                                meio = new DateTime(dtBilhete[0], dtBilhete[1], dtBilhete[2]);

                                if ((meio > ini) && (meio < fin))
                                {
                                    listBoxBilhetesCliente.Items.Add("Bilhete do voo " + control.Bilhetes[i].CodVoo);
                                    listBoxBilhetesCliente.Items.Add("Data: " + control.Voos[j].DataVoo);
                                    listBoxBilhetesCliente.Items.Add("Tipo: " + control.Voos[j].Id);
                                    listBoxBilhetesCliente.Items.Add("Origem: " + control.Voos[j].CidOrigem);
                                    listBoxBilhetesCliente.Items.Add("Destino: " + control.Voos[j].CidDestino);
                                    listBoxBilhetesCliente.Items.Add("");
                                    aux++;
                                }
                            }
                        }
                    }
                }

                if (aux == 0)
                {
                    listBoxBilhetesCliente.Items.Add("Cliente " + control.Clientes[pos].Nome + " não possui bilhetes");
                    listBoxBilhetesCliente.Items.Add("entre " + dateTimePickerBilClienteIni.Text + " e " + dateTimePickerBilClienteFin.Text);
                }
                labelInfo.Text = "Listando bilhetes emitidos para " + control.Clientes[pos].Nome + " entre " + dateTimePickerBilClienteIni.Text + " e " + dateTimePickerBilClienteFin.Text;
            }

            catch (ExceptionError1)
            {
                labelInfo.Text = "Um cliente deve ser selecionado";
            }
        }
    }
}