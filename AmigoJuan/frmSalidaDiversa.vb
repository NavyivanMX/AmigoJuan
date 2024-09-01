Imports System.Data.SqlClient

Public Class frmSalidaDiversa
    Dim DTP As New DataTable
    Dim DV2 As New DataView
    Dim DT2 As New DataTable
    Dim LGRU As New List(Of String)
    Dim LPRO As New List(Of String)

    Dim LDES As New List(Of String)
    Dim LCON As New List(Of String)
    Dim LCLI As New List(Of String)

    Dim CEDIS, NCEDIS, EMP As String
    Dim DV As New DataView

    Dim DTA As New DataTable
    Dim DTT As New DataTable
    Dim VLOTE As Integer

    Private Sub frmSalidaDiversa_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)
        DTT.Columns.Add("Lote")
        DTT.Columns.Add("Producto")
        DTT.Columns.Add("Cantidad", GetType(Double))
        DTT.Columns.Add("Unidad", GetType(String))
        DTT.Columns.Add("Conteo", GetType(String))
        DTT.Columns.Add("Ubicación", GetType(String))
        DTT.Columns.Add("Existencia", GetType(Double))
        DTT.Columns.Add("CLote", GetType(String))
        If Me.LLAMACEDIS Then
            Me.Icon = frmPrincipal.Icon
            LLENACOMBOBOX2(CBCLI, LCLI, "SELECT CLAVE,NOMBRE FROM CLIENTES WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
            LLENACOMBOBOX2(CBCON, Me.LCON, "SELECT CLAVE,NOMBRE FROM MOVIMIENTOSINVENTARIO WHERE ORIENTACION=2 AND ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
            CARGANOENTRADA()
            ACTUALIZADGV()
        Else
            Me.Close()
        End If
    End Sub

    Private Function LLAMACEDIS() As Boolean
        Me.Opacity = 25
        Dim almacen As New frmLlamaAlmacen
        almacen.MOSTRAR(0)
        If (almacen.DialogResult = DialogResult.Yes) Then
            Me.CEDIS = almacen.ALM
            Me.NCEDIS = almacen.NALM
            Me.EMP = almacen.EMP
            Me.Text = ("Salida Diversa para: " & Me.NCEDIS)
            Return True
        End If
        Return False
    End Function

    Private Function CARGANOENTRADA() As Integer
        Dim num As Integer
        If Not frmPrincipal.CHECACONX Then
            Return num
        End If
        Dim reader As SqlDataReader = New SqlCommand(("SELECT MAX(NOSALIDA)ORDEN FROM SALIDASALMACENES WHERE ALMACEN='" & Me.CEDIS & "'"), frmPrincipal.CONX).ExecuteReader
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
                TOT += DGV.Item(2, X).Value
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
        If CBCLI.SelectedIndex = 0 Then
            MessageBox.Show("Debe seleccionar un cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If CBCON.SelectedIndex = 0 Then
            MessageBox.Show("Debe seleccionar un concepto de salida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Not VALIDACANTIDADES() Then
            MessageBox.Show("Favor de verificar las cantidades en cero y existencia", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim num As Short = CShort(MessageBox.Show("¿Desea Guardar la Información?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        If (num = 6) Then
            Me.GUARDAR()
        End If
    End Sub
    Private Function VALIDACANTIDADES() As Boolean
        Dim X As Integer
        Dim CANT, EXIS As Double
        For X = 0 To DGV.Rows.Count - 1
            Try
                CANT = DGV.Item(2, X).Value
            Catch ex As Exception
                CANT = 0
            End Try
            Try
                EXIS = DGV.Item(6, X).Value
            Catch ex As Exception
                EXIS = 0
            End Try
            If CANT <= 0 Then
                DGV.CurrentCell = DGV.Rows(X).Cells(1)
                Return False
            End If
            If CANT > EXIS Then
                Return False
            End If
        Next
        Return True
    End Function
    Dim NOO As Integer
    Private Sub GUARDAR()
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If
        NOO = CARGANOENTRADA()
        Dim SQL As New SqlClient.SqlCommand("INSERT INTO SALIDASALMACENES (ALMACEN,NOSALIDA,CONCEPTO,CLIENTE,FECHA,USUARIO,OBSERVACION,FOLIO) VALUES (@ALM,@NOO,@CON,@CLI,@FEC,@USU,@OBS,@FOLIO)", frmPrincipal.CONX)
        SQL.Parameters.Add("@ALM", SqlDbType.Int).Value = CEDIS
        SQL.Parameters.Add("@NOO", SqlDbType.Int).Value = NOO
        SQL.Parameters.Add("@CON", SqlDbType.Int).Value = LCON(CBCON.SelectedIndex)
        SQL.Parameters.Add("@FEC", SqlDbType.DateTime).Value = DTFECHA.Value
        SQL.Parameters.Add("@USU", SqlDbType.VarChar).Value = frmPrincipal.Usuario
        SQL.Parameters.Add("@OBS", SqlDbType.VarChar).Value = TXTOBS.Text
        SQL.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = TXTFOLIO.Text
        SQL.Parameters.Add("@CLI", SqlDbType.Int).Value = LCLI(CBCLI.SelectedIndex)
        SQL.ExecuteNonQuery()


        Dim SQLD As New SqlClient.SqlCommand("INSERT INTO DETALLESALIDASALMACENES (ALMACEN,NOSALIDA,LOTE,CANTIDAD,REGISTRO) VALUES (@ALM,@NOO,@LOTE,@CANT,@REG)", frmPrincipal.CONX)
        SQLD.Parameters.Add("@ALM", SqlDbType.Int).Value = CEDIS
        SQLD.Parameters.Add("@NOO", SqlDbType.Int).Value = NOO

        SQLD.Parameters.Add("@LOTE", SqlDbType.VarChar)
        SQLD.Parameters.Add("@CANT", SqlDbType.Float)
        SQLD.Parameters.Add("@REG", SqlDbType.Int)
        Dim X As Integer
        For X = 0 To DGV.Rows.Count - 1
            Try
                SQLD.Parameters("@LOTE").Value = DGV.Item(7, X).Value.ToString
                SQLD.Parameters("@CANT").Value = DGV.Item(2, X).Value
                SQLD.Parameters("@REG").Value = X
                SQLD.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Next
        Dim SQLK As New SqlClient.SqlCommand("KARDEXPORSALIDA", frmPrincipal.CONX)
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

        CARGANOENTRADA()
    End Sub

    Private Sub LIMPIAR()
        MODULOGENERAL.LimpiarForm(Me.Controls)
        Me.TXTOBS.Text = ""
        TXTFOLIO.Text = ""
    End Sub

    Private Sub IMPRIMIR()
        Dim rEP As New rptSalidaDiversa
        Dim QUERY As String
        QUERY = "SELECT A.NOMBRE ALMACEN,E.NOSALIDA,C.NOMBRE CONCEPTO,AREA='',E.FECHA,US.NOMBRE USUARIO,P.NOMBRECORTO PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.CANTIDAD*L.COSTO TOTAL,E.OBSERVACION,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,CLI.NOMBRE CLIENTE,L.COSTO,E.FOLIO,E.FECHAREG FROM SALIDASALMACENES E INNER JOIN ALMACENES A ON E.ALMACEN=A.CLAVE INNER JOIN DETALLESALIDASALMACENES D ON E.ALMACEN=D.ALMACEN AND E.NOSALIDA=D.NOSALIDA INNER JOIN MOVIMIENTOSINVENTARIO C ON E.CONCEPTO=C.CLAVE  INNER JOIN LOTESP L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 INNER JOIN USUARIOS US ON E.USUARIO=US.USUARIO INNER JOIN CLIENTES CLI ON E.CLIENTE=CLI.CLAVE WHERE E.ALMACEN='" + CEDIS + "' AND E.NOSALIDA = " + NOO.ToString + " ORDER BY P.NOMBRE,L.TEMPORADA,L.LOTE"
        MODULOGENERAL.MOSTRARREPORTE(rEP, ("Salida Diversa " & Me.LBLPED.Text), MODULOGENERAL.LLENATABLA(QUERY, frmPrincipal.CadenaConexion), "")
    End Sub


    Private Sub BTNMOSTRAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNMOSTRAR.Click
        frmBusquedaSeleccionMultiple.BUSCAR("SELECT D.PRODUCTO CLAVE,L.NOMBRECORTO LOTE,P.NOMBRE PRODUCTO,D.CANTIDAD,L.RENDIMIENTO CONTEO,L.UBICACION,L.LOTE LOTETEMP FROM INVALMACENES D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE", " WHERE P.NOMBRE", " ORDER BY P.NOMBRE,L.TEMPORADA,L.LOTE", "Selección de Productos", "Nombre del Producto", "Producto(s)", 2, frmPrincipal.CadenaConexion, True, DTP, 1)
        If frmBusquedaSeleccionMultiple.DialogResult = Windows.Forms.DialogResult.Yes Then
            DTA = New DataTable
            DTA = frmBusquedaSeleccionMultiple.TREG
            'DGV.DataSource = DTA
            ACTUALIZASELECCION()
        End If
    End Sub
    Private Sub ACTUALIZASELECCION()
        Dim X, Y As Integer
        Dim ENC As Boolean
        Dim z As Integer
        z = DTA.Columns.Count - 1
        Dim algo As Boolean
        For X = 0 To DTA.Rows.Count - 1
            ENC = False
            algo = CType(DTA.Rows(X).Item(z), Boolean)
            If CType(DTA.Rows(X).Item(z), Boolean) Then
                For Y = 0 To DTT.Rows.Count - 1
                    If DTA.Rows(X).Item(0).ToString = DTT.Rows(Y).Item(7).ToString Then
                        ENC = True
                        'DTT.Rows(Y).Item(3) = DTA.Rows(X).Item(3)
                    End If
                Next
            Else
                ENC = True
            End If

            If Not ENC Then
                Dim DOW As System.Data.DataRow = DTT.NewRow
                DOW(0) = DTA.Rows(X).Item(1).ToString
                DOW(1) = DTA.Rows(X).Item(2).ToString
                DOW(2) = 0
                DOW(3) = "Kg"
                DOW(4) = DTA.Rows(X).Item(4).ToString
                DOW(5) = DTA.Rows(X).Item(5).ToString
                DOW(6) = DTA.Rows(X).Item(3).ToString
                DOW(7) = DTA.Rows(X).Item(0).ToString

                DTT.Rows.Add(DOW)
            End If
        Next
        ACTUALIZADGV()
    End Sub
    Private Sub ACTUALIZADGV()
        DGV.DataSource = DTT
        DGV.Columns(7).Visible = False
        DGV.Columns(0).ReadOnly = True
        DGV.Columns(1).ReadOnly = True
        DGV.Columns(3).ReadOnly = True

        DGV.Columns(4).ReadOnly = True
        DGV.Columns(5).ReadOnly = True
        DGV.Columns(6).ReadOnly = True

        DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        DGV.Columns(2).DefaultCellStyle = FORMATONUMERICOND(3)
        DGV.Columns(6).DefaultCellStyle = FORMATONUMERICOND(3)

        CHECATABLA()
    End Sub

    Private Sub DGV_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV.CellEndEdit
        CHECATABLA()
    End Sub
End Class