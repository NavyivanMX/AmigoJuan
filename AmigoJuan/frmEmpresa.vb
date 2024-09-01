Public Class frmEmpresa
    Inherits System.Windows.Forms.Form
    Dim LGRU As New List(Of String)
    ''---> PROGRAMALO PARA K CON ENTER SE PASE AL SIGUIENTE... PERO NOOOOOO COMO MI ABUELA...
    Private Sub BTNGUARDAR_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BTNGUARDAR.BackColor = System.Drawing.Color.Olive
        BTNGUARDAR.Text = "GUARDAR"
        BTNGUARDAR.ForeColor = System.Drawing.Color.Green
    End Sub

    Private Sub BTNGUARDAR_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BTNGUARDAR.BackColor = System.Drawing.Color.Transparent
        BTNGUARDAR.Text = ""
        BTNGUARDAR.ForeColor = System.Drawing.Color.White
    End Sub

    Private Sub frmEmpresa_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        'Me.BackgroundImage = frmPrincipal.BackgroundImage
        'BackgroundImageLayout = ImageLayout.Stretch
        VISUALIZACION(Me)
        LLENACOMBOBOX(CBGRU, LGRU, "SELECT CLAVE,NOMBRE FROM GRUPOSEMPRESAS ORDER BY NOMBRE", frmPrincipal.CadenaConexion)
        ACTIVAR(True)
        LIMPIAR()
    End Sub

    Private Sub ACTIVAR(ByVal V As Boolean)
        TXTCLA.Enabled = V
        TXTNF.Enabled = Not V
        CBGRU.Enabled = Not V
        CBACT.Enabled = Not V
        BTNEXA.Enabled = Not V
        TXTNC.Enabled = Not V
        TXTDIR.Enabled = Not V
        TXTRFC.Enabled = Not V
        BTNELIMINAR.Enabled = False
        BTNGUARDAR.Enabled = Not V
        If V Then
            TXTCLA.Focus()
            TXTCLA.SelectAll()
        Else
            TXTNF.Focus()
            TXTNF.SelectAll()
        End If
    End Sub

    Private Sub LIMPIAR()
        TXTNF.Text = ""
        CBACT.SelectedIndex = 0
        PBSEC.Image = Nothing
        TXTCLA.Focus()
        TXTCLA.SelectAll()
    End Sub

    Private Sub CARGADATOS()
        If Not frmPrincipal.CHECACONX() Then
            Exit Sub
        End If
        ACTIVAR(False)
        Dim SQLSELECT As New SqlClient.SqlCommand("SELECT CLAVE,NOMBREFISCAL,NOMBRECOMUN,RFC,DIRECCION,ACTIVO,IMAGEN,GRUPO FROM EMPRESASGASTOS WHERE CLAVE=@CLA", frmPrincipal.CONX)
        SQLSELECT.Parameters.Add("@CLA", SqlDbType.Int).Value = TXTCLA.Text
        Dim LECTOR As SqlClient.SqlDataReader
        LECTOR = SQLSELECT.ExecuteReader
        If LECTOR.Read Then
            TXTNF.Text = LECTOR(1)
            TXTNC.Text = LECTOR(2)
            TXTRFC.Text = LECTOR(3)
            TXTDIR.Text = LECTOR(4)
            If CType(LECTOR(5), Boolean) Then
                CBACT.SelectedIndex = 1
            Else
                CBACT.SelectedIndex = 0
            End If
            Try
                PBSEC.Image = Bytes2Image(LECTOR(6))
            Catch ex As Exception

            End Try
            CARGAX(LGRU, CBGRU, LECTOR(7))
            BTNELIMINAR.Enabled = True
        End If
        LECTOR.Close()
    End Sub

    Private Sub GUARDAR()
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If
        Dim SQL As New SqlClient.SqlCommand("SPEMPRESASGASTOS", frmPrincipal.CONX)
        SQL.CommandType = CommandType.StoredProcedure
        SQL.Parameters.Add("@CLA", SqlDbType.VarChar).Value = TXTCLA.Text
        SQL.Parameters.Add("@NF", SqlDbType.VarChar).Value = TXTNF.Text
        SQL.Parameters.Add("@NC", SqlDbType.VarChar).Value = TXTNC.Text
        SQL.Parameters.Add("@RFC", SqlDbType.VarChar).Value = TXTRFC.Text
        SQL.Parameters.Add("@DIR", SqlDbType.VarChar).Value = TXTDIR.Text
        SQL.Parameters.Add("@IMG", SqlDbType.Image).Value = Image2Bytes(PBSEC.Image)
        SQL.Parameters.Add("@ACT", SqlDbType.Bit).Value = CBACT.SelectedIndex
        SQL.Parameters.Add("@GRU", SqlDbType.Int).Value = LGRU(CBGRU.SelectedIndex)
        SQL.ExecuteNonQuery()
        MessageBox.Show("La Información ha sido Guardada correctamente", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        ACTIVAR(True)
        LIMPIAR()
    End Sub

    Private Sub BTNGUARDAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNGUARDAR.Click
        Dim xyz As Short
        xyz = MessageBox.Show("¿Desea Guardar la Información?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If xyz <> 6 Then
            Exit Sub
        End If
        GUARDAR()
    End Sub

    Private Sub BTNELIMINAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNELIMINAR.Click
        Dim xyz As Short
        xyz = MessageBox.Show("¿Esta seguro que desea Eliminar?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If xyz = 6 Then
            Try
                Dim SQLELIMINAR As New SqlClient.SqlCommand
                SQLELIMINAR.Connection = frmPrincipal.CONX
                SQLELIMINAR.CommandText = "DELETE FROM EMPRESASGASTOS WHERE CLAVE='" + TXTCLA.Text + "'"
                SQLELIMINAR.ExecuteNonQuery()
                MessageBox.Show("La Información ha sido Eliminada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Catch ex As Exception
                MessageBox.Show("La Empresa Gasto no puede ser eliminado, ya que existen Platillos dentro de esta Sección", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End Try
            LIMPIAR()
            ACTIVAR(True)
        End If
    End Sub

    Private Sub BTNCANCELAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCANCELAR.Click
        ACTIVAR(True)
        LIMPIAR()
    End Sub

    Private Sub TXTCLA_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TXTCLA.KeyPress
        If e.KeyChar = Chr(13) Then
            If TXTCLA.Text = "" Then
            Else
                CARGADATOS()
            End If
        End If
    End Sub

    Private Sub BTNBUSCAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNBUSCAR.Click
        frmClsBusqueda.BUSCAR("SELECT CLAVE,NOMBREFISCAL,NOMBRECOMUN,ACTIVO FROM EMPRESASGASTOS ", " WHERE NOMBREFISCAL", " ORDER BY NOMBREFISCAL", "Búsqueda de Empresas Gastos", "Nombre Empresa", "Empresa(s)", 1, frmPrincipal.CadenaConexion, True)
        If frmClsBusqueda.DialogResult = Windows.Forms.DialogResult.Yes Then
            TXTCLA.Text = frmClsBusqueda.TREG.Rows(0).Item(0).ToString
            CARGADATOS()
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNEXA.Click
        Dim ofd As New OpenFileDialog
        ofd.Title = "Archivos de imagen JPG"
        ofd.DefaultExt = ".jpg,*.png"
        ofd.Filter = "Archivos de imagen (*.jpg)|*.jpg|Archivos de Imagen(*.png)|*.png|Bitmap (*.bmp)|*.bmp"
        ofd.FilterIndex = 1
        ofd.FileName = ""

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.PBSEC.Image = Bitmap.FromFile(ofd.FileName)
        End If
    End Sub
End Class