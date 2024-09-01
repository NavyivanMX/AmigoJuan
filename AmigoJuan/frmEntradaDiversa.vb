Imports System.Data.SqlClient

Public Class frmEntradaDiversa
    Dim DTP As New DataTable
    Dim DV2 As New DataView
    Dim DT2 As New DataTable
    Dim LGRU As New List(Of String)
    Dim LPRO As New List(Of String)

    Dim LDES As New List(Of String)
    Dim LCON As New List(Of String)
    Dim LPROVE As New List(Of String)
    Dim LTEMP As New List(Of String)

    Dim LAREA As New List(Of String)

    Dim CEDIS, NCEDIS, EMP As String
    Dim DV As New DataView

    Dim DTA As New DataTable
    Dim DTT As New DataTable
    Dim VLOTE As Integer
    Private Sub frmEntradaDiversa_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)

        DTT.Columns.Add("Producto")
        DTT.Columns.Add("Unidad")
        DTT.Columns.Add("Lote", GetType(String))
        DTT.Columns.Add("Cantidad", GetType(Double))
        DTT.Columns.Add("Costo", GetType(Double))
        DTT.Columns.Add("Total", GetType(Double))
        DTT.Columns.Add("Conteo por Lb / Kg", GetType(String))
        DTT.Columns.Add("Ubicación", GetType(String))
        DTT.Columns.Add("CProducto", GetType(Integer))
        DTT.Columns.Add("CEmpresa", GetType(Integer))
        DTT.Columns.Add("CTemporada", GetType(Integer))
        DTT.Columns.Add("CLote", GetType(Integer))

        If Me.LLAMACEDIS Then
 
            Me.Icon = frmPrincipal.Icon
            Dim QUERY As String
            LLENACOMBOBOX2(CBPROVE, LPROVE, "SELECT CLAVE,NOMBRE FROM PROVEEDORES WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
            LLENACOMBOBOX(CBTEMP, LTEMP, "SELECT CLAVE,NOMBRE FROM TEMPORADAS WHERE ACTIVO=1 ORDER BY CLAVE", frmPrincipal.CadenaConexion)
            LLENACOMBOBOX2(CBCON, Me.LCON, "SELECT CLAVE,NOMBRE FROM MOVIMIENTOSINVENTARIO WHERE ORIENTACION=1 AND ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
            CARGAAUTO()
            QUERY = "SELECT P.CLAVE,P.NOMBRE,P.NOMBRECORTO,P.FAMILIA FROM PRODUCTOS P WHERE P.ACTIVO=1"
            frmPrincipal.DT = LLENATABLA((QUERY), frmPrincipal.CadenaConexion)
            LLENACOMBOBOX(CBGRU, LGRU, "SELECT CLAVE,NOMBRE FROM FAMILIAS WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion)
            CARGANOENTRADA()
            ACTUALIZADGV()
        Else
            Me.Close()
        End If
    End Sub
    Private Sub CARGAAUTO()
        If CHKAUTO.Checked Then

            Try
                CBTEMP.SelectedIndex = CBTEMP.Items.Count - 1
            Catch ex As Exception

            End Try
            Try
                If Not frmPrincipal.CHECACONX Then
                    Exit Sub
                End If
                Dim SQL As New SqlClient.SqlCommand("SELECT DBO.SIGLOTETEMPORADA('" + LTEMP(CBTEMP.SelectedIndex) + "')", frmPrincipal.CONX)
                Dim LEC As SqlClient.SqlDataReader
                LEC = SQL.ExecuteReader
                If LEC.Read Then
                    TXTLOTE.Text = LEC(0)
                End If
                LEC.Close()
                SQL.Dispose()

            Catch ex As Exception

            End Try

        End If
        CBTEMP.Enabled = Not CHKAUTO.Checked
        TXTLOTE.Enabled = Not CHKAUTO.Checked
        REFRESCALT()
    End Sub
    Private Sub REFRESCALT()
        If Not VERIFICALOTE() Then
            Exit Sub
        End If
        Dim X As Integer
        For X = 0 To DGV.Rows.Count - 1
            DGV.Item(10, X).Value = LTEMP(CBTEMP.SelectedIndex)
            DGV.Item(11, X).Value = VLOTE
        Next
        CHECATABLA()
    End Sub
    Private Function LLAMACEDIS() As Boolean
        Me.Opacity = 25
        Dim almacen As New frmLlamaAlmacen
        almacen.MOSTRAR(0)
        If (almacen.DialogResult = DialogResult.Yes) Then
            Me.CEDIS = almacen.ALM
            Me.NCEDIS = almacen.NALM
            Me.EMP = almacen.EMP
            Me.Text = ("Entrada Diversa para: " & Me.NCEDIS)
            Return True
        End If
               Return False
    End Function

    Private Function CARGANOENTRADA() As Integer
        Dim num As Integer
        If Not frmPrincipal.CHECACONX Then
            Return num
        End If
        Dim reader As SqlDataReader = New SqlCommand(("SELECT MAX(NOORDEN)ORDEN FROM ENTRADASALMACENES WHERE ALMACEN='" & Me.CEDIS & "'"), frmPrincipal.CONX).ExecuteReader
        If Not reader.Read Then
            Return num
        End If
        If (reader.Item(0) Is DBNull.Value) Then
            reader.Close()
            Me.LBLPED.Text = "1"
            Return 1
        End If
        Dim num2 As Integer = (Convert.ToInt32(reader.Item(0)) + 1)
        Me.LBLPED.Text = num2.ToString
        reader.Close()
        Return num2
    End Function

    Private Function CARGADATAVIEW(ByVal GRUPO As String) As Boolean
        Me.DV = New DataView(frmPrincipal.DT, ("FAMILIA='" & GRUPO & "'"), "NOMBRE", DataViewRowState.CurrentRows)
        Return Me.CARGAPRODUCTOS
    End Function

    Private Function CARGAPRODUCTOS() As Boolean
        Dim flag As Boolean
        Me.CBPROD.Items.Clear()
        Me.LPRO.Clear()
        LDES.Clear()
        CBPROD.Items.Add("Favor de Seleccionar")
        LPRO.Add("")
        LDES.Add("")
        Dim num3 As Integer = (Me.DV.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num3)
            Dim view As DataRowView = Me.DV.Item(i)
            Me.CBPROD.Items.Add(view.Item(2))
            Me.LPRO.Add(view.Item(0).ToString)
            Me.LDES.Add(view.Item(1).ToString)
            i += 1
        Loop
        Dim count As Integer = Me.CBPROD.Items.Count
        Try
            Me.CBPROD.SelectedIndex = 0
            flag = True
        Catch exception1 As Exception
            MessageBox.Show(exception1.Message)
            flag = False
            Return flag
        End Try
        Return flag
    End Function

    Private Sub ACTIVAR(ByVal V As Boolean)
        Try
            Me.TXTCANT.Enabled = V
            'Me.CBUNI.Enabled = V
            Me.BTNAGREGAR.Enabled = V
        Catch exception1 As Exception
            'frmPrincipal.CE.ESCRIBIR("Sailda inventario restaurantes", DateAndTime.Now, "ventana:salidainventariorestaurante metodo: ACTIVAR", exception1.Message.ToString)
            Return
        End Try
    End Sub

    Private Sub CBGRU_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBGRU.SelectedIndexChanged
        Me.ACTIVAR(True)
        If Not Me.CARGADATAVIEW(Me.LGRU.Item(Me.CBGRU.SelectedIndex)) Then
            MessageBox.Show("No Se encuentra Asignado Ningún Producto en este Grupo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Me.CBPROD.Enabled = False
            Me.ACTIVAR(False)
        Else
            Me.CBPROD.Enabled = True
        End If
    End Sub

    Private Function PRODUCTOAGREGADO(ByVal PRO As String) As Boolean
        Dim num2 As Integer = (Me.DTT.Rows.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            If Me.DTT.Rows.Item(i).Item(7) = PRO Then
                Return True
            End If
            i += 1
        Loop
        Return False
    End Function

    Private Sub BTNQUITAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNQUITAR.Click
        Me.DTT.Rows.RemoveAt(Me.DGV.CurrentRow.Index)
        Me.CHECATABLA()
    End Sub

    Dim TOT As Double
    Private Sub CHECATABLA()
        TOT = 0
        If DGV.Rows.Count = 0 Then
            BTNQUITAR.Enabled = False
            BTNELIMINAR.Enabled = False
            BTNGUARDAR.Enabled = False
        Else
            Dim X As Integer
            For X = 0 To DGV.Rows.Count - 1
                Dim LOTE As Integer
                LOTE = DGV.Item(11, X).Value
                Dim algo As String
                algo = LOTE.ToString("000")
                DGV.Item(2, X).Value = "L" + DGV.Item(10, X).Value.ToString.Substring(2, 2) + "-" + algo
                TOT += DGV.Item(3, X).Value
            Next

            'SET @NC='L'+SUBSTRING (CONVERT(VARCHAR(10),@TEMP),LEN(CONVERT(VARCHAR(10),@TEMP))-1,2)+'-'+DBO.LOTE23DIG(@LOTE)
            'LBLTOT.Text = FormatNumber(TOT, 2).ToString
            LBLCUANTOS.Text = FormatNumber(TOT, 2).ToString
            BTNGUARDAR.Enabled = True
            BTNQUITAR.Enabled = True
            BTNELIMINAR.Enabled = True
        End If
    End Sub

    Private Sub BTNELIMINAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNELIMINAR.Click
        Dim num As Short = CShort(MessageBox.Show("¿Esta seguro que desea Eliminar TODOS los elementos Agregados?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
        If (num = 6) Then
            Me.DTT.Rows.Clear()
            Me.DGV.Refresh()
            Me.CHECATABLA()
        End If
    End Sub

    Private Sub BTNGUARDAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNGUARDAR.Click
        If CBPROVE.SelectedIndex = 0 Then
            MessageBox.Show("Debe seleccionar un proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If CBCON.SelectedIndex = 0 Then
            MessageBox.Show("Debe seleccionar un concepto de entrada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Not VALIDACANTIDADES() Then
            MessageBox.Show("Favor de verificar las cantidades", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Not VALIDAUBICACIONES() Then
            MessageBox.Show("Favor de verificar las ubicaciones", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim num As Short = CShort(MessageBox.Show("¿Desea Guardar la Información?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        If (num = 6) Then
            Me.GUARDAR()
        End If
    End Sub
    Private Function VALIDACANTIDADES() As Boolean
        Dim X As Integer
        Dim CANT, TOT As Double
        For X = 0 To DGV.Rows.Count - 1
            Try
                CANT = DGV.Item(3, X).Value
            Catch ex As Exception
                CANT = 0
            End Try
            Try
                TOT = DGV.Item(5, X).Value
            Catch ex As Exception
                TOT = 0
            End Try
            If CANT <= 0 Then
                DGV.CurrentCell = DGV.Rows(X).Cells(3)
                Return False
            End If
            If TOT < 0 Then
                DGV.CurrentCell = DGV.Rows(X).Cells(5)
                Return False
            End If
        Next
        Return True
    End Function
    Private Function VALIDAUBICACIONES() As Boolean
        Dim X As Integer
        For X = 0 To DGV.Rows.Count - 1
            If DGV.Item(7, X).Value.ToString.Trim() = "" Then
                DGV.CurrentCell = DGV.Rows(X).Cells(7)
                Return False
            End If
        Next
        Return True
    End Function
    Dim NOO As Integer
    Private Function CARGASIGLOTE(ByVal EMP As Integer, ByVal TEMP As Integer, ByVal LOTE As Integer, ByVal PRO As Integer, ByVal REN As String, ByVal UBI As String) As String
        Dim REG As String
        REG = "N/A"
        If Not frmPrincipal.CHECACONX Then
            Return "N/A"
        End If
        Dim LEC As SqlDataReader = New SqlCommand("SELECT DBO.SIGLOTEP('" + EMP.ToString + "','" + TEMP.ToString + "','" + LOTE.ToString + "','" + PRO.ToString + "','" + REN + "','" + UBI + "')", frmPrincipal.CONX).ExecuteReader
        If LEC.Read Then
            REG = LEC(0)
        End If
        LEC.Close()
        Return REG
    End Function
    Private Sub GUARDAR()
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If


        NOO = CARGANOENTRADA()

        Dim SQLLT As New SqlClient.SqlCommand("SPLOTETEMPORADA", frmPrincipal.CONX)
        SQLLT.CommandType = CommandType.StoredProcedure
        SQLLT.CommandTimeout = 300
        SQLLT.Parameters.Add("@TEMP", SqlDbType.Int).Value = LTEMP(CBTEMP.SelectedIndex)
        SQLLT.Parameters.Add("@LOTE", SqlDbType.Int).Value = VLOTE
        SQLLT.Parameters.Add("@USU", SqlDbType.VarChar).Value = frmPrincipal.Usuario
        SQLLT.ExecuteNonQuery()

        Dim SQL As New SqlClient.SqlCommand("INSERT INTO ENTRADASALMACENES (ALMACEN,NOORDEN,CONCEPTO,TOTAL,FECHA,USUARIO,OBSERVACION,PROVEEDOR) VALUES (@ALM,@NOO,@CON,@TOT,@FEC,@USU,@OBS,@PROVE)", frmPrincipal.CONX)
        SQL.Parameters.Add("@ALM", SqlDbType.Int).Value = CEDIS
        SQL.Parameters.Add("@NOO", SqlDbType.Int).Value = NOO
        SQL.Parameters.Add("@CON", SqlDbType.Int).Value = LCON(CBCON.SelectedIndex)
        SQL.Parameters.Add("@TOT", SqlDbType.Float).Value = TOT
        SQL.Parameters.Add("@FEC", SqlDbType.DateTime).Value = DTFECHA.Value
        SQL.Parameters.Add("@USU", SqlDbType.VarChar).Value = frmPrincipal.Usuario
        SQL.Parameters.Add("@OBS", SqlDbType.VarChar).Value = TXTOBS.Text
        SQL.Parameters.Add("@PROVE", SqlDbType.Int).Value = LPROVE(CBPROVE.SelectedIndex)
        SQL.ExecuteNonQuery()

        Dim SQLL As New SqlClient.SqlCommand("SPLOTESP", frmPrincipal.CONX)
        SQLL.CommandType = CommandType.StoredProcedure
        SQLL.CommandTimeout = 300
        SQLL.Parameters.Add("@EMP", SqlDbType.Int).Value = frmPrincipal.Empresa
        SQLL.Parameters.Add("@TEMP", SqlDbType.Int)
        SQLL.Parameters.Add("@LOTE", SqlDbType.Int)
        SQLL.Parameters.Add("@PRO", SqlDbType.Int)
        SQLL.Parameters.Add("@PROV", SqlDbType.Int).Value = LPROVE(CBPROVE.SelectedIndex)
        SQLL.Parameters.Add("@USU", SqlDbType.VarChar).Value = frmPrincipal.Usuario
        SQLL.Parameters.Add("@COS", SqlDbType.Float)
        SQLL.Parameters.Add("@REN", SqlDbType.VarChar)
        SQLL.Parameters.Add("@UBI", SqlDbType.VarChar)
        Dim SQLD As New SqlClient.SqlCommand("INSERT INTO DETALLEENTRADASALMACENES (ALMACEN,NOORDEN,LOTE,CANTIDAD,UNIDAD,TOTAL,REGISTRO) VALUES (@ALM,@NOO,@LOTE,@CANT,@UNI,@TOT,@REG)", frmPrincipal.CONX)
        SQLD.Parameters.Add("@ALM", SqlDbType.Int).Value = CEDIS
        SQLD.Parameters.Add("@NOO", SqlDbType.Int).Value = NOO
        SQLD.Parameters.Add("@UNI", SqlDbType.Int).Value = 1

        SQLD.Parameters.Add("@LOTE", SqlDbType.VarChar)
        SQLD.Parameters.Add("@CANT", SqlDbType.Float)
        SQLD.Parameters.Add("@TOT", SqlDbType.Float)
        SQLD.Parameters.Add("@REG", SqlDbType.Int)
        Dim X As Integer
        Dim COS As Double
        Dim SIGLOTE As String
        SIGLOTE = "N/A"
        For X = 0 To DGV.Rows.Count - 1
            Try
                SIGLOTE = CARGASIGLOTE(frmPrincipal.Empresa, DGV.Item(10, X).Value.ToString, DGV.Item(11, X).Value.ToString, DGV.Item(8, X).Value.ToString, DGV.Item(6, X).Value.ToString, DGV.Item(7, X).Value.ToString)
            Catch ex As Exception

            End Try

            Try
                SQLL.Parameters("@TEMP").Value = DGV.Item(10, X).Value.ToString
                SQLL.Parameters("@LOTE").Value = DGV.Item(11, X).Value.ToString
                SQLL.Parameters("@PRO").Value = DGV.Item(8, X).Value.ToString
                COS = DGV.Item(4, X).Value
                SQLL.Parameters("@COS").Value = COS
                SQLL.Parameters("@REN").Value = DGV.Item(6, X).Value.ToString
                SQLL.Parameters("@UBI").Value = DGV.Item(7, X).Value.ToString
                SQLL.ExecuteNonQuery()
            Catch ex As Exception

            End Try

            Try
                Dim LOTE As Integer
                LOTE = DGV.Item(11, X).Value
                Dim algo As String
                algo = LOTE.ToString("000")
                DGV.Item(2, X).Value = "L" + DGV.Item(10, X).Value.ToString.Substring(2, 2) + "-" + algo

                'SQLD.Parameters("@LOTE").Value = "E" + frmPrincipal.Empresa + "T" + DGV.Item(10, X).Value.ToString + "L" + LOTE.ToString + "P" + DGV.Item(8, X).Value.ToString

                SQLD.Parameters("@LOTE").Value = SIGLOTE
                SQLD.Parameters("@CANT").Value = DGV.Item(3, X).Value
                SQLD.Parameters("@TOT").Value = DGV.Item(5, X).Value
                SQLD.Parameters("@REG").Value = X
                SQLD.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Next
        Dim SQLK As New SqlClient.SqlCommand("KARDEXPORENTRADA", frmPrincipal.CONX)
        SQLK.CommandType = CommandType.StoredProcedure
        SQLK.CommandTimeout = 300
        SQLK.Parameters.Add("@ALM", SqlDbType.Int).Value = CEDIS
        SQLK.Parameters.Add("@NOC", SqlDbType.Int).Value = NOO
        SQLK.ExecuteNonQuery()

        MessageBox.Show("La información ha sido guardada con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        IMPRIMIR()
        LIMPIAR()
        DTT.Rows.Clear()
        ACTUALIZADGV()
        TXTCANT.Text = ""
        TXTTOT.Text = ""
        TXTREN.Text = ""
        TXTUBI.Text = ""
        CARGANOENTRADA()
        CHKAUTO.Checked = False
        CHKAUTO.Checked = True
        CBPROD.Focus()
    End Sub

    Private Sub LIMPIAR()
        MODULOGENERAL.LimpiarForm(Me.Controls)
        Me.TXTCANT.Text = ""
        Me.TXTOBS.Text = ""
        Me.TXTTOT.Text = ""
    End Sub

    Private Sub IMPRIMIR()
        Dim rEP As New rptEntradaDiversa
        Dim QUERY As String
        QUERY = "SELECT A.NOMBRE ALMACEN,E.NOORDEN,C.NOMBRE CONCEPTO,AREA='',E.FECHA,US.NOMBRE USUARIO,P.NOMBRECORTO PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.TOTAL,OBSERVACION OBSERVACIONES,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,PROVE.NOMBRE PROVEEDOR,L.COSTO,E.FECHAREG FROM ENTRADASALMACENES E INNER JOIN ALMACENES A ON E.ALMACEN=A.CLAVE INNER JOIN DETALLEENTRADASALMACENES D ON E.ALMACEN=D.ALMACEN AND E.NOORDEN=D.NOORDEN INNER JOIN MOVIMIENTOSINVENTARIO C ON E.CONCEPTO=C.CLAVE  INNER JOIN LOTESP L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON D.UNIDAD=U.CLAVE INNER JOIN USUARIOS US ON E.USUARIO=US.USUARIO INNER JOIN PROVEEDORES PROVE ON E.PROVEEDOR=PROVE.CLAVE WHERE E.ALMACEN='" + CEDIS + "' AND E.NOORDEN = " + NOO.ToString
        MODULOGENERAL.MOSTRARREPORTE(rEP, ("Entrada Diversa " & Me.LBLPED.Text), MODULOGENERAL.LLENATABLA(QUERY, frmPrincipal.CadenaConexion), "")
    End Sub

    Private Sub frmEntradaDiversa_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.F3) Then
            frmClsBusqueda.BUSCAR("SELECT CLAVE,Nombre,FAMILIA,ACTIVO FROM PRODUCTOS", " WHERE NOMBRE", " ORDER BY NOMBRE", "Búsqueda de Productos", "Nombre del Producto", "Productos(s)", 1, frmPrincipal.CadenaConexion, False)
            If (frmClsBusqueda.DialogResult = DialogResult.Yes) Then
                Dim cBGRU As ComboBox = Me.CBGRU
                MODULOGENERAL.CARGAX(Me.LGRU, cBGRU, frmClsBusqueda.TREG.Rows.Item(0).Item(2).ToString)
                Me.CBGRU = cBGRU
                cBGRU = Me.CBPROD
                MODULOGENERAL.CARGAX(Me.LPRO, cBGRU, frmClsBusqueda.TREG.Rows.Item(0).Item(0).ToString)
                Me.CBPROD = cBGRU
                Me.TXTCANT.Focus()
                Me.TXTCANT.SelectAll()
            End If
        End If
    End Sub

    Private Sub BTNMOSTRAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNMOSTRAR.Click
        If Not VERIFICALOTE() Then
            MessageBox.Show("Lote no válido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        frmBusquedaSeleccionMultiple.BUSCAR("SELECT P.CLAVE,P.NOMBRE,NOMBRECORTO FROM PRODUCTOS P WHERE ACTIVO=1 ", " AND P.NOMBRE", " ORDER BY P.NOMBRE", "Selección de Productos", "Nombre del Producto", "Producto(s)", 1, frmPrincipal.CadenaConexion, True, DTP)
        If frmBusquedaSeleccionMultiple.DialogResult = Windows.Forms.DialogResult.Yes Then
            DTA = New DataTable
            DTA = frmBusquedaSeleccionMultiple.TREG
            'DGV.DataSource = DTA
            ACTUALIZASELECCION()
        End If

    End Sub
    Private Function VERIFICALOTE() As Boolean
        Try
            VLOTE = CType(TXTLOTE.Text, Integer)

        Catch ex As Exception
            Return False
        End Try
        If VLOTE <= 0 Then
            Return False
        End If
        Return True
    End Function
    Private Sub ACTUALIZASELECCION()

        Dim X, Y As Integer
        Dim ENC As Boolean
        Dim z As Integer
        z = DTA.Columns.Count - 1
        For X = 0 To DTA.Rows.Count - 1
            ENC = False

            For Y = 0 To DTT.Rows.Count - 1
                If DTA.Rows(X).Item(0).ToString = DTT.Rows(Y).Item(8).ToString And CType(DTA.Rows(X).Item(z), Boolean) Then
                    ENC = True
                    'DTT.Rows(Y).Item(3) = DTA.Rows(X).Item(3)
                End If
            Next
            If Not ENC Then
                Dim DOW As System.Data.DataRow = DTT.NewRow
                DOW(0) = DTA.Rows(X).Item(2).ToString
                DOW(1) = "Kg"
                DOW(2) = ""
                DOW(3) = 0
                DOW(4) = 0
                DOW(5) = 0
                DOW(6) = ""
                DOW(7) = ""
                DOW(8) = DTA.Rows(X).Item(0).ToString
                DOW(9) = frmPrincipal.Empresa
                DOW(10) = LTEMP(CBTEMP.SelectedIndex)
                DOW(11) = VLOTE

                DTT.Rows.Add(DOW)
            End If
        Next
        ACTUALIZADGV()
   
    End Sub
    Private Sub ACTUALIZADGV()
        DGV.DataSource = DTT
        DGV.Columns(11).Visible = False
        DGV.Columns(10).Visible = False
        DGV.Columns(9).Visible = False
        DGV.Columns(8).Visible = False
        DGV.Columns(0).ReadOnly = True
        DGV.Columns(1).ReadOnly = True
        DGV.Columns(2).ReadOnly = True
        DGV.Columns(5).ReadOnly = True

        DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        DGV.Columns(3).DefaultCellStyle = FORMATONUMERICOND(3)
        DGV.Columns(4).DefaultCellStyle = FORMATONUMERICO()
        DGV.Columns(5).DefaultCellStyle = FORMATONUMERICO()

        CHECATABLA()
    End Sub


    Private Sub CBGRU_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TXTCANT.KeyPress, CBPROD.KeyPress, CBGRU.KeyPress, TXTTOT.KeyPress, TXTREN.KeyPress, TXTUBI.KeyPress
        If (e.KeyChar = ChrW(13)) Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CHKAUTO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CHKAUTO.CheckedChanged
        CARGAAUTO()
    End Sub

    Private Sub CBPROD_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBPROD.SelectedIndexChanged
        Try
            LBLDES.Text = LDES(CBPROD.SelectedIndex)
        Catch ex As Exception

        End Try
    End Sub


    Private Sub BTNAGREGAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNAGREGAR.Click
        If CBPROD.SelectedIndex = 0 Then
            MessageBox.Show("Favor de seleccionar un producto", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            Exit Sub
        End If
        AGREGAR()
    End Sub
    Private Sub AGREGAR()
        If PRODUCTOAGREGADO(LPRO(CBPROD.SelectedIndex)) Then
            MessageBox.Show("Producto ya agregado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Not VERIFICALOTE() Then
            MessageBox.Show("Lote no válido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim CANT, COS As Double
        Try
            CANT = CType(TXTCANT.Text, Double)
            COS = CType(TXTTOT.Text, Double)
        Catch ex As Exception
            MessageBox.Show("Cantidades no válidas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        If CANT <= 0 Then
            MessageBox.Show("Cantidades no válidas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim DOW As System.Data.DataRow = DTT.NewRow
        DOW(0) = CBPROD.Text
        DOW(1) = "Kg"
        DOW(2) = ""
        DOW(3) = CANT
        DOW(4) = COS
        DOW(5) = CANT * COS
        DOW(6) = TXTREN.Text
        DOW(7) = TXTUBI.Text
        DOW(8) = LPRO(CBPROD.SelectedIndex)
        DOW(9) = frmPrincipal.Empresa
        DOW(10) = LTEMP(CBTEMP.SelectedIndex)
        DOW(11) = VLOTE

        DTT.Rows.Add(DOW)
        ACTUALIZADGV()
        TXTCANT.Text = ""
        TXTTOT.Text = ""
        TXTREN.Text = ""
        TXTUBI.Text = ""
        CBPROD.Focus()
    End Sub

    Private Sub CBTEMP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CBTEMP.SelectedIndexChanged
        REFRESCALT()
    End Sub

    Private Sub TXTLOTE_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXTLOTE.TextChanged
        REFRESCALT()
    End Sub
    Private Sub TOTALES()
        Dim X As Integer
        TOT = 0
        For X = 0 To DGV.Rows.Count - 1
            DGV.Item(5, X).Value = DGV.Item(3, X).Value * DGV.Item(4, X).Value
            TOT += DGV.Item(3, X).Value
        Next
        LBLCUANTOS.Text = FormatNumber(TOT, 2).ToString
    End Sub
    Private Sub DGV_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV.CellEndEdit
        TOTALES()
    End Sub
End Class