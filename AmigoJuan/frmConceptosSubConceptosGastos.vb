Public Class frmConceptosSubConceptosGastos
   
    Dim TABLAPRIN As New DataTable
    Dim CLACON As New List(Of String)
    Dim CLASC As New List(Of String)
    Private Sub CBSC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        CBACT.SelectedIndex = 0

        TABLAPRIN.Columns.Clear()
        'TABLAPRIN.Columns.Add("CLAVE CONCEPTO")
        'TABLAPRIN.Columns.Add("CONCEPTO")
        TABLAPRIN.Columns.Add("CLAVE")
        TABLAPRIN.Columns.Add("SUB CONCEPTO")
        TABLAPRIN.Columns.Add("ACTIVO")

        CARGACONCEPTO()
        CARGASUBCONCEPTO()

        ACTIVAR2(True)
        ACTIVAR(True)

    End Sub
    Private Function CARGACONCEPTO() As Boolean
        CBCON.Items.Clear()
        CLACON.Clear()
        Dim SQLCARGAGPO As New SqlClient.SqlCommand("SELECT CLAVE,NOMBRE FROM CONCEPTOSGASTOS WHERE ACTIVO=1 AND EMPRESA=" + frmPrincipal.Empresa + " ORDER BY NOMBRE", frmPrincipal.CONX)
        Dim LECTOR As SqlClient.SqlDataReader
        LECTOR = SQLCARGAGPO.ExecuteReader
        While LECTOR.Read
            CLACON.Add(LECTOR(0))
            CBCON.Items.Add(LECTOR(1))
        End While
        LECTOR.Close()
        Try
            CBCON.SelectedIndex = 0
            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function
    Private Function CARGASUBCONCEPTO() As Boolean
        CBSC.Items.Clear()
        CLASC.Clear()
        Dim SQLCARGAGPO As New SqlClient.SqlCommand("SELECT CLAVE,NOMBRE FROM SUBCONCEPTOSGASTOS WHERE ACTIVO=1 AND EMPRESA=" + frmPrincipal.Empresa + " ORDER BY NOMBRE", frmPrincipal.CONX)
        Dim LECTOR As SqlClient.SqlDataReader
        LECTOR = SQLCARGAGPO.ExecuteReader
        While LECTOR.Read
            CLASC.Add(LECTOR(0))
            CBSC.Items.Add(LECTOR(1))
        End While
        LECTOR.Close()
        Try
            CBSC.SelectedIndex = 0
            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function

    Private Sub ACTIVAR2(ByVal V As Boolean)
        BTNAGREGAR.Enabled = V
        BTNQUITAR.Enabled = Not V
        BTNELIMINAR.Enabled = Not V
        BTNGUARDAR.Enabled = Not V
    End Sub
    Private Sub ACTIVAR(ByVal V As Boolean)
        CBCON.Enabled = V
        CBSC.Enabled = Not V
        CBACT.Enabled = Not V
        BTNAGREGAR.Enabled = Not V
        BTNCANCELAR.Enabled = Not V
    End Sub
    Private Sub LIMPIAR()
        CBCON.SelectedIndex = 0
        CBSC.SelectedIndex = 0
        CBACT.SelectedIndex = 0
    End Sub

    Private Sub AGREGAR()
        DGV.Rows.Add(1)
        Dim ITEMS As Integer
        ITEMS = DGV.Rows.Count - 1
        DGV.Item(0, ITEMS).Value = CLASC(CBSC.SelectedIndex)
        DGV.Item(1, ITEMS).Value = CBSC.Text
        If CBACT.SelectedIndex = 0 Then
            DGV.Item(2, ITEMS).Value = True
        Else
            DGV.Item(2, ITEMS).Value = False
        End If
        DGV.Refresh()

        DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        BTNQUITAR.Enabled = True
        BTNGUARDAR.Enabled = True
        DGV.Refresh()
    End Sub
    Private Sub BTNQUITAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNQUITAR.Click
        DGV.Rows.RemoveAt(DGV.CurrentRow.Index)
        CHECATABLA()
    End Sub
    Private Sub BTNAGREGAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNAGREGAR.Click

        Dim x As Integer
        For x = 0 To DGV.Rows.Count - 1
            If DGV.Item(0, x).Value.ToString = CLASC(CBSC.SelectedIndex) Then
                MessageBox.Show("El sub concepto ya fue agregado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Next
        AGREGAR()
    End Sub
    Private Sub CHECATABLA()
        If DGV.Rows.Count = 0 Then
            BTNQUITAR.Enabled = False
            BTNELIMINAR.Enabled = False
            BTNGUARDAR.Enabled = False
        Else
            BTNQUITAR.Enabled = True
            BTNELIMINAR.Enabled = True
            BTNGUARDAR.Enabled = True
        End If
    End Sub
    Private Sub GUARDAR()

        Dim SQL As New SqlClient.SqlCommand("DELETE FROM CONCEPTOSUBCONCEPTO WHERE CONCEPTO='" + CLACON(CBCON.SelectedIndex) + "' AND EMPRESA=" + frmPrincipal.Empresa + "", frmPrincipal.CONX)
        SQL.ExecuteNonQuery()
        Dim SQLD As New SqlClient.SqlCommand("INSERT INTO CONCEPTOSUBCONCEPTO(EMPRESA,CONCEPTO,SUBCONCEPTO,ACTIVO)VALUES(@EMP,@CON,@SC,@ACT)", frmPrincipal.CONX)
        SQLD.Parameters.Add("@CON", SqlDbType.VarChar)
        SQLD.Parameters.Add("@SC", SqlDbType.VarChar)
        SQLD.Parameters.Add("@ACT", SqlDbType.Bit)
        SQLD.Parameters.Add("@EMP", SqlDbType.Int).Value = frmPrincipal.Empresa

        Dim X As Integer
        'Dim ABC As String
        For X = 0 To DGV.Rows.Count - 1
            SQLD.Parameters("@CON").Value = CLACON(CBCON.SelectedIndex)
            SQLD.Parameters("@SC").Value = DGV.Item(0, X).Value
            If DGV.Item(2, X).Value = True Then
                SQLD.Parameters("@ACT").Value = True
            Else
                SQLD.Parameters("@ACT").Value = False
            End If

            SQLD.ExecuteNonQuery()
        Next

        MessageBox.Show("La información ha sido guardada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        DGV.Rows.Clear()
        ACTIVAR2(True)
        ACTIVAR(True)
    End Sub
    Private Sub CARGADATOS()
        Dim LAQ As String

        LAQ = "SELECT CSC.CONCEPTO,C.NOMBRE,CSC.SUBCONCEPTO,SC.NOMBRE,CSC.ACTIVO FROM CONCEPTOSUBCONCEPTO CSC INNER JOIN CONCEPTOSGASTOS C ON C.CLAVE=CSC.CONCEPTO AND C.EMPRESA=CSC.EMPRESA INNER JOIN SUBCONCEPTOSGASTOS SC ON SC.CLAVE=CSC.SUBCONCEPTO AND SC.EMPRESA=CSC.EMPRESA  WHERE CSC.CONCEPTO='" + CLACON(CBCON.SelectedIndex) + "' AND CSC.EMPRESA=" + frmPrincipal.Empresa + ""
        Dim DA As New SqlClient.SqlDataAdapter(LAQ, frmPrincipal.CONX)
        Dim DDT As New DataTable
        DA.Fill(DDT)
        DGV.Rows.Clear()
        Dim X As Integer
        For X = 0 To DDT.Rows.Count - 1
            DGV.Rows.Add(1)
            Dim ITEMS As Integer
            ITEMS = DGV.Rows.Count - 1
            DGV.Item(0, ITEMS).Value = DDT.Rows(X).Item(2)
            DGV.Item(1, ITEMS).Value = DDT.Rows(X).Item(3)
            If CType(DDT.Rows(X).Item(4), Boolean) Then
                DGV.Item(2, ITEMS).Value = True
            Else
                DGV.Item(2, ITEMS).Value = False
            End If
        Next
        DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        ACTIVAR(False)
        CHECATABLA()
    End Sub

   
    Private Sub CBCON_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CBCON.KeyPress
        If e.KeyChar = Chr(13) Then
            CARGADATOS()
        End If
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCANCELAR.Click
        ACTIVAR2(True)
        ACTIVAR(True)

        If DGV.Rows.Count >= 1 Then
            DGV.Rows.RemoveAt(DGV.CurrentRow.Index)
        End If
    End Sub

    Private Sub BTNGUARDAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNGUARDAR.Click
        GUARDAR()
        CBCON.Focus()
    End Sub

    Private Sub BTNELIMINAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNELIMINAR.Click
        Dim xyz As Short
        xyz = MessageBox.Show("¿Estas seguro que deseas eliminar los elementos agregados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If xyz <> 6 Then
            Exit Sub
        End If

        Dim SQL As New SqlClient.SqlCommand("DELETE FROM CONCEPTOSUBCONCEPTO WHERE CONCEPTO='" + CLACON(CBCON.SelectedIndex) + "' AND EMPRESA=" + frmPrincipal.Empresa + "", frmPrincipal.CONX)
        SQL.ExecuteNonQuery()
        DGV.Rows.Clear()
        DGV.Refresh()
        CHECATABLA()
        ACTIVAR2(True)
        ACTIVAR(True)
        CBCON.Focus()

    End Sub
End Class