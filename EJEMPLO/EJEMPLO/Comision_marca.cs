using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using FuncionesNavegador;
//using dllconsultas;
using MySql.Data.MySqlClient;
using System.Data.Odbc;
using MySql.Data.MySqlClient;
//using seguridad;

namespace EJEMPLO
{
    public partial class Comision_marca : Form
    {
        public Comision_marca()
        {
            InitializeComponent();
        }
        //CapaNegocio fn = new CapaNegocio();
        
        //FuncionesNavegador.CapaNegocio fn = new FuncionesNavegador.CapaNegocio();
        String Codigo;
        String atributo;
        Boolean Editar = false;
        private static string id_form = "13204";
        //bitacora bita = new bitacora();
        //DataTable seg = seguridad.ObtenerPermisos.Permisos(seguridad.Conexion.User, id_form);
        int id_fact;
        decimal total;
        private void Comision_marca_Load(object sender, EventArgs e)
        {
            /*
            fn.InhabilitarComponentes(gpb_com_ven);
            fn.InhabilitarComponentes(this);
            
           
            */
            llenaridempresa();
            llenarvendedor();
        }
        

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            
        }

        private void llenaridempresa()
        {

            //se realiza la conexión a la base de datos
            Conexionmysql.ObtenerConexion();
            //se inicia un DataSet
            DataSet ds = new DataSet();
            //se indica la consulta en sql
            String Query = "select id_empresa_pk, nombre from empresa Where estado <>'INACTIVO'";
            MySqlDataAdapter dad = new MySqlDataAdapter(Query, Conexionmysql.ObtenerConexion());
            //se indica con quu tabla se llena
            dad.Fill(ds, "empresa");
            cbo_empres.DataSource = ds.Tables[0].DefaultView;
            //indicamos el valor de los miembros
            cbo_empres.ValueMember = ("id_empresa_pk");
            //se indica el valor a desplegar en el combobox
            cbo_empres.DisplayMember = ("nombre");
            Conexionmysql.Desconectar();
        }

        private void llenarvendedor()
        {

            string selectedItem = cbo_empres.SelectedValue.ToString();
            //se realiza la conexión a la base de datos
            Conexionmysql.ObtenerConexion();
            //se inicia un DataSet
            DataSet ds = new DataSet();
            //se indica la consulta en sql=
            String Query = "select id_empleados_pk, nombre from empleado where id_empresa_pk ='" + selectedItem + "' AND estado <>'INACTIVO' AND cargo ='Vendedor' AND cargo = 'VENDEDOR'";
            MySqlDataAdapter dad = new MySqlDataAdapter(Query, Conexionmysql.ObtenerConexion());
            //se indica con quu tabla se llena
            dad.Fill(ds, "empleado");
            cbo_empleado.DataSource = ds.Tables[0].DefaultView;
            //indicamos el valor de los miembros
            cbo_empleado.ValueMember = ("id_empleados_pk");
            //se indica el valor a desplegar en el combobox
            cbo_empleado.DisplayMember = ("nombre");
            Conexionmysql.Desconectar();
        }

        private void btn_emp_Click(object sender, EventArgs e)
        {
            cbo_empleado.Text = "  ";
            llenarvendedor();

        }

        
        public void factura()
        {
            int factura = 0;
            string serie;
            decimal total_f = 0;
            string descripcion;
            decimal precio_b = 0;
            int cantidad = 0;
            string nom_marca;
            decimal comisiones = 0;
            int cont1 = 0;
            string selectedItem = cbo_empres.SelectedValue.ToString();
            string selectedItem2 = cbo_empleado.SelectedValue.ToString();
            string date1 = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string date2 = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            MySqlCommand Query = new MySqlCommand();
            MySqlConnection Conexion;
            MySqlDataReader consultar;
            string sql = "server=localhost;user id=root;database=hotelsancarlos;password=";
            //string sql = "dsn=hotelsancarlos;server=localhost;database=hotelsancarlos;uid=root;password=";
           // string sql = "dsn=hotelsancarlos;server=192.168.0.120;database=hotelsancarlos; uid=Otto; pwd=090113290;";
            Conexion = new MySqlConnection();
            Conexion.ConnectionString = sql;
            Conexion.Open();
            //Query.CommandText = "SELECT id_fac_empresa_pk From factura where id_empleados_pk = '" + selectedItem2 + "'And id_empresa_pk ='" + selectedItem + "' AND estado <> 'INACTIVO' AND marca_comision <>'S' AND fecha_emision BETWEEN '" + date1 + "' AND '" + date2 + "';";
            Query.CommandText = "SELECT DISTINCT FD.id_fac_empresa_pk,F.serie, F.total, B.descripcion, PR.precio, FD.cantidad, b.porcentaje_comision FROM empresa E, empleado EMP, factura F, factura_detalle FD, precio PR, bien B WHERE(E.id_empresa_pk = 1 AND EMP.id_empleados_pk = 1 AND F.id_empleados_pk = EMP.id_empleados_pk AND FD.id_fac_empresa_pk = F.id_fac_empresa_pk AND PR.id_precio = FD.id_precio AND PR.id_bien_pk = B.id_bien_pk); ";
            //Query.CommandText = "SELECT DISTINCT FD.id_fac_empresa_pk,F.serie, F.total, B.descripcion, PR.precio, FD.cantidad, M.nombre_marca, M.procentaje_comision FROM empresa E, empleado EMP, factura F, factura_detalle FD, precio PR, bien B, marca M WHERE(E.id_empresa_pk = '"+selectedItem+"' AND EMP.id_empleados_pk = '"+selectedItem2+ "' AND F.id_empleados_pk = EMP.id_empleados_pk AND FD.id_fac_empresa_pk = F.id_fac_empresa_pk AND PR.id_precio = FD.id_precio AND PR.id_bien_pk = B.id_bien_pk AND B.id_marca_pk = M.id_marca_pk AND F.fecha_emision BETWEEN '" + date1 + "' AND '" + date2 + "' AND F.marca_comision <> 'MARCA' AND F.marca_comision <> 'LINEA' AND F.marca_comision <> 'PRODUCTO' AND F.marca_comision <>'GLOBAL' AND B.id_categoria_pk='PT');";
            Query.Connection = Conexion;
            consultar = Query.ExecuteReader();

            while (consultar.Read())
            {
               dataGridView1.Rows.Add(1);

                //id_fact = consultar.GetInt32(0);
                //MessageBox.Show(Convert.ToString(id_fact));
                if (cont1 == 0)
                {
                   // dataGridView2.Rows.Add(1);
                    factura = consultar.GetInt32(0);
                    MessageBox.Show(Convert.ToString(factura));
                    serie = consultar.GetString(1);
                    total_f = consultar.GetDecimal(2);
                    descripcion = consultar.GetString(3);
                    precio_b = consultar.GetDecimal(4);
                    cantidad = consultar.GetInt32(5);
                    nom_marca = consultar.GetString(6);
                   // comisiones = consultar.GetDecimal(7);
                    dataGridView1.Rows[0].Cells[0].Value = factura;
                    dataGridView1.Rows[0].Cells[1].Value = serie;
                    dataGridView1.Rows[0].Cells[2].Value = total_f;
                    dataGridView1.Rows[0].Cells[3].Value = descripcion;
                    dataGridView1.Rows[0].Cells[4].Value = precio_b;
                    dataGridView1.Rows[0].Cells[5].Value = cantidad;
                    dataGridView1.Rows[0].Cells[6].Value = nom_marca;
                    //dataGridView1.Rows[0].Cells[7].Value = comisiones;
                    //dataGridView1.Rows[0].Cells[1].Value = total;
                    // 
                }
                else
                {

                    factura = consultar.GetInt32(0);
                    serie = consultar.GetString(1);
                    total_f = consultar.GetDecimal(2);
                    descripcion = consultar.GetString(3);
                    precio_b = consultar.GetDecimal(4);
                    cantidad = consultar.GetInt32(5);
                    nom_marca = consultar.GetString(6);
                   // comisiones = consultar.GetDecimal(7);
                    dataGridView1.Rows[cont1].Cells[0].Value = factura;
                    dataGridView1.Rows[cont1].Cells[1].Value = serie;
                    dataGridView1.Rows[cont1].Cells[2].Value = total_f;
                    dataGridView1.Rows[cont1].Cells[3].Value = descripcion;
                    dataGridView1.Rows[cont1].Cells[4].Value = precio_b;
                    dataGridView1.Rows[cont1].Cells[5].Value = cantidad;
                    dataGridView1.Rows[cont1].Cells[6].Value = nom_marca;
                    //dataGridView1.Rows[cont1].Cells[7].Value = comisiones;
                }
                cont1++;
                
            }
        }
        
        
        


        private void cbo_empres_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbo_empleado.Text = "  ";
            llenarvendedor();
        }

        public void comision()
        {

            decimal total_comision = 0;
            for (int fila = 0; fila < dataGridView1.RowCount-1; fila++)
            {

                total_comision = Convert.ToDecimal(dataGridView1.Rows[fila].Cells[8].Value) * ((Convert.ToDecimal(dataGridView1.Rows[fila].Cells[7].Value) / 100));
                dataGridView1.Rows[fila].Cells[9].Value = Math.Round(total_comision,2);
            }

        }

        public void totales()
        {
            decimal total_venta = 0;

            for (int fila = 0; fila < dataGridView1.RowCount-1; fila++)
            {

                total_venta = Convert.ToDecimal(dataGridView1.Rows[fila].Cells[4].Value) * Convert.ToDecimal(dataGridView1.Rows[fila].Cells[5].Value);
                dataGridView1.Rows[fila].Cells[8].Value = Math.Round(total_venta,2);
            }

            //txt_total_com.Text = Convert.ToString(total_comi);
            //txt_venta.Text = Convert.ToString(total_venta);

        }
        public void comision_total()
        {
            decimal comision_t = 0;
            for (int fila = 0; fila < dataGridView1.RowCount-1 ; fila++)
            {

                comision_t += Convert.ToDecimal(dataGridView1.Rows[fila].Cells[9].Value);

            }
            txt_total_com.Text = Convert.ToString(comision_t);

        }
        public void venta_total()
        {
            decimal venta_t = 0;
            for (int fila = 0; fila < dataGridView1.RowCount-1; fila++)
            {

                venta_t += Convert.ToDecimal(dataGridView1.Rows[fila].Cells[8].Value);

            }
            txt_venta.Text = Convert.ToString(venta_t);
        }


        private void btn_nuevo_Click(object sender, EventArgs e)
        { /*
            try
            {
                Editar = false;
                fn.ActivarControles(gpb_com_ven);
                fn.LimpiarComponentes(gpb_com_ven);
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;
                button1.Enabled = false;
                txt_total_com.Enabled = false;
                txt_venta.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */
        }

        private void btn_cancelar_Click_1(object sender, EventArgs e)
        {/*
            try
            {
                Editar = false;
                fn.LimpiarComponentes(gpb_com_ven);
                fn.InhabilitarComponentes(gpb_com_ven);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            factura();
            
            totales();
            comision();
            venta_total();
            comision_total();
            
            
        }

        public void guardar_detalle()
        {
            /*
            string selectedItem = cbo_empres.SelectedValue.ToString();
            string selectedItem2 = cbo_empleado.SelectedValue.ToString();
            textBox5.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            textBox8.Text = selectedItem;
            //DataRow permiso = seg.Rows[0];
            textBox7.Text = selectedItem2;

            //CapaNegocio fn = new CapaNegocio();
            FuncionesNavegador.CapaNegocio fn = new FuncionesNavegador.CapaNegocio();
            TextBox[] textbox = { txt_total_com,txt_venta,textBox5,textBox8, textBox7};
            DataTable datos = fn.construirDataTable(textbox);
            if (datos.Rows.Count == 0)
            {
                //MessageBox.Show("Hay campos vacios", "Favor Verificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string tabla = "detalle_com_ventas";
                if (Editar)
                {
                    fn.modificar(datos, tabla, atributo, Codigo);
                }
                else
                {
                    fn.insertar(datos, tabla);
                    bita.Insertar("Insercion de comisiones del empleado: " + cbo_empleado.Text, "detalle_com_venta");
                }
                fn.LimpiarComponentes(this);
            }
            */
        }


        private void btn_guardar_Click(object sender, EventArgs e)
        {
            /*
                for (int fila = 0; fila < dataGridView1.RowCount - 1; fila++)
                {
                    DataRow permiso = seg.Rows[0];
                    int insertar = Convert.ToInt32(permiso[0]);
                    string selectedItem = cbo_empres.SelectedValue.ToString();
                    string selectedItem2 = cbo_empleado.SelectedValue.ToString();
                    textBox1.Text = selectedItem;
                    textBox2.Text = Convert.ToString(dataGridView1.Rows[fila].Cells[0].Value);
                    textBox3.Text = Convert.ToString(dataGridView1.Rows[fila].Cells[7].Value);
                    textBox4.Text = Convert.ToString(dataGridView1.Rows[fila].Cells[8].Value);
                    textBox5.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    textBox6.Text = Convert.ToString(dataGridView1.Rows[fila].Cells[9].Value);
                    textBox7.Text = selectedItem2;

                    //CapaNegocio fn = new CapaNegocio();
                    FuncionesNavegador.CapaNegocio fn = new FuncionesNavegador.CapaNegocio();
                    TextBox[] textbox = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7 };
                    DataTable datos = fn.construirDataTable(textbox);
                    if (datos.Rows.Count == 0)
                    {
                        //MessageBox.Show("Hay campos vacios", "Favor Verificar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string tabla = "com_venta";
                        if (Editar)
                        {
                            fn.modificar(datos, tabla, atributo, Codigo);
                        }
                        else
                        {

                            textBox10.Text = "MARCA";
                            fn.insertar(datos, tabla);
                            bita.Insertar("Insercion de comisiones del empleado: " + cbo_empleado.Text, "com_venta");
                            atributo = "id_fac_empresa_pk";
                            string tabla2 = "factura";
                            Codigo = textBox2.Text;
                            //CapaNegocio fn = new CapaNegocio();
                            TextBox[] textbox2 = { textBox10 };
                            DataTable datos2 = fn.construirDataTable(textbox2);
                            fn.modificar(datos2, tabla2, atributo, Codigo);
                            bita.Modificar("Se modifico marca_comision a MARCA", "factura");
                        }
                        fn.LimpiarComponentes(this);
                    }
                }
                guardar_detalle();

                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                MessageBox.Show("Operacion exitosa", "Confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            */
           
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = true;
            dateTimePicker3.Enabled = true;
            button1.Enabled = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cbo_empleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            txt_total_com.Text = "";
            txt_venta.Text = "";
        }

        private void cbo_empres_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            txt_total_com.Text = "";
            txt_venta.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {/*
            string selectedItem2 = cbo_empleado.SelectedValue.ToString();
            Facturas a = new Facturas(cbo_empleado.Text, selectedItem2);

            a.MdiParent = this.ParentForm;
            a.Show();*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CRITICO!!!!!");
        }
    }
}
