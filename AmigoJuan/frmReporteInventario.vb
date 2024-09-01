Public Class frmReporteInventario
    Dim CEDIS, NCEDIS, EMP As String
    Private Sub frmReporteInventario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)
        If Not LLAMACEDIS() Then
            Me.Close()
        Else
            RB1.Checked = True
            CARGARDATOS()
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
            Me.Text = ("Reporte de Inventario para: " & Me.NCEDIS)
            Return True
        End If
        Return False
    End Function
    Private Sub CARGARDATOS()
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If
        Dim QUERY As String
        If RB1.Checked Then
            QUERY = "SELECT P.NOMBRE PRODUCTO,SUM(D.CANTIDAD)EXISTENCIA,P.CLAVE FROM INVALMACENES D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE WHERE ALMACEN='" + CEDIS + "' GROUP BY P.NOMBRE,P.CLAVE"
            DGV.DataSource = LLENATABLA(QUERY, frmPrincipal.CadenaConexion)
        Else
            QUERY = "SELECT P.NOMBRE PRODUCTO,SUM(D.CANTIDAD)EXISTENCIA,P.CLAVE FROM HISTINV D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE WHERE ALMACEN='" + CEDIS + "' AND D.FECHA=@INI GROUP BY P.NOMBRE,P.CLAVE"
            DGV.DataSource = LLENATABLAIF(QUERY, frmPrincipal.CadenaConexion, DTDE.Value.Date, DTDE.Value.Date)
        End If

        DGV.Columns(2).Visible = False
        DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV.Columns(1).DefaultCellStyle = FORMATONUMERICOND(3)
    End Sub
    Private Sub CARGARDETALLE(ByVal CP As String)
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If
        Dim QUERY As String
        If RB1.Checked Then
            QUERY = "SELECT P.NOMBRECORTO PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.CANTIDAD*L.COSTO TOTAL,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,L.COSTO,L.CLAVE FROM INVALMACENES D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE WHERE D.ALMACEN='" + CEDIS + "' AND P.CLAVE=" + CP
            DGV2.DataSource = LLENATABLA(QUERY, frmPrincipal.CadenaConexion)
        Else
            QUERY = "SELECT P.NOMBRECORTO PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.CANTIDAD*L.COSTO TOTAL,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,L.COSTO,L.CLAVE FROM HISTINV D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE WHERE D.ALMACEN='" + CEDIS + "'  AND D.FECHA=@INI  AND P.CLAVE=" + CP
            DGV2.DataSource = LLENATABLAIF(QUERY, frmPrincipal.CadenaConexion, DTDE.Value.Date, DTDE.Value.Date)
        End If
        'QUERY = "SELECT P.NOMBRECORTO PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.CANTIDAD*L.COSTO TOTAL,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,L.COSTO,L.CLAVE FROM INVALMACENES D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE WHERE D.ALMACEN='" + CEDIS + "' AND P.CLAVE=" + CP
        'DGV2.DataSource = LLENATABLA(QUERY, frmPrincipal.CadenaConexion)
        DGV2.Columns(8).Visible = False
        DGV2.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV2.Columns(1).DefaultCellStyle = FORMATONUMERICOND(3)

        DGV2.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV2.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(7).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV2.Columns(1).DefaultCellStyle = FORMATONUMERICOND(3)
        DGV2.Columns(3).DefaultCellStyle = FORMATONUMERICOND(3)
    End Sub

    Private Sub DGV_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV.CellDoubleClick
        If DGV.Rows.Count <= 0 Then
        Else
            CARGARDETALLE(DGV.Item(2, DGV.CurrentRow.Index).Value.ToString)
        End If
    End Sub

    Private Sub BTNIMPRIMIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNIMPRIMIR.Click
        Dim REP As New rptReporteInventario
        Dim QUERY As String
        If RB1.Checked Then
            QUERY = "SELECT * FROM VRINVENTARIO ORDER BY PRODUCTO,TEMPORADA,LOTE"
            MOSTRARREPORTE(REP, "Reporte de Inventario", LLENATABLA(QUERY, frmPrincipal.CadenaConexion), "")
        Else
            QUERY = "SELECT A.NOMBRE ALMACEN,P.NOMBRECORTO PRODUCTO,D.CANTIDAD, U.NOMBRE UNIDAD,D.CANTIDAD*L.COSTO TOTAL,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,L.COSTO,P.ORDEN,PR.NOMBRE PROVEEDOR,L.TEMPORADA,L.LOTE,OBSERVACION='Inventario a la Fecha: " + Format(DTDE.Value.Date, "dd/MM/yyyy") + "' FROM HISTINV D INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE INNER JOIN PROVEEDORES PR ON L.PROVEEDOR=PR.CLAVE WHERE D.FECHA=@INI ORDER BY P.NOMBRE,L.TEMPORADA,L.LOTE"
            MOSTRARREPORTE(REP, "Reporte de Inventario", LLENATABLAIF(QUERY, frmPrincipal.CadenaConexion, DTDE.Value.Date, DTDE.Value.Date), "")
        End If
    End Sub

    Private Sub DGV2_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV2.CellDoubleClick
        If DGV2.Rows.Count <= 0 Then
            Exit Sub
        End If
        Dim VMU As New frmModificarUbicacion
        VMU.MOSTRAR(DGV2.Item(4, DGV2.CurrentRow.Index).Value.ToString, DGV2.Item(0, DGV2.CurrentRow.Index).Value.ToString, DGV2.Item(6, DGV2.CurrentRow.Index).Value.ToString, DGV2.Item(8, DGV2.CurrentRow.Index).Value.ToString, DGV2.Item(7, DGV2.CurrentRow.Index).Value.ToString, DGV2.Item(5, DGV2.CurrentRow.Index).Value.ToString)
        If VMU.DialogResult = Windows.Forms.DialogResult.Yes Then
            CARGARDETALLE(DGV.Item(2, DGV.CurrentRow.Index).Value.ToString)
        End If
        VMU.Dispose()
    End Sub

    Private Sub BTNMOSTRAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNMOSTRAR.Click
        CARGARDATOS()
    End Sub

    Private Sub RB1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RB1.CheckedChanged, RB2.CheckedChanged
        DTDE.Enabled = RB2.Checked
    End Sub
End Class