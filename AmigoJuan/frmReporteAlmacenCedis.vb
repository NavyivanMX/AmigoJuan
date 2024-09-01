Public Class frmReporteAlmacenCedis
    Dim LALM As New List(Of String)
    Dim CONZ As New SqlClient.SqlConnection
    Dim CADENAZ As String

    Private Sub frmReporteAlmacenCedis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)
        CADENAZ = "Data Source=54.152.213.58,51433;Network Library=DBMSSOCN;Initial Catalog=BMSVB-AWS;User ID=bms;Password=4mvb201809-AdiO5._#"
        CONZ.ConnectionString = CADENAZ
        CONZ.Open()
        CHECATABLA()
        LLENACOMBOBOX2(CBALM, LALM, "	SELECT COD_ESTAB,NOMBRE FROM establecimientos WHERE  STATUS='V' AND NOMBRE LIKE '%CEDIS%' ORDER BY NOMBRE", CADENAZ, "Favor de Seleeccionar", "")
    End Sub
    Private Sub CARGADATOS()
        If CBALM.SelectedIndex = 0 Then
            MessageBox.Show("Favor de seleccionar un almacén", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            If CONZ.State = ConnectionState.Closed Or CONZ.State = ConnectionState.Broken Then
                CONZ.Open()
            End If
            CONZ.Open()
        Catch ex As Exception

        End Try

        Dim QUERY As String
        QUERY = "select a.fecha Fecha, a.folio Folio, a.cod_prod [Codigo Producto], c.descripcion Descripcion, cast((round(a.cantidad,0)) as int)Cantidad, g.notas Destino, ar.nombre [Responsable Area] from dbo.entysal (nolock) as a inner join dbo.movimientos_internos (nolock) as b on b.folio=a.folio inner join dbo.productos (nolock) as c on c.cod_prod=a.cod_prod inner join dbo.establecimientos (nolock) as d on d.cod_estab=b.cod_estab_alterno left join dbo.pedestab (nolock) as g on g.folio=b.pedido inner join Areas (nolock)  ar on c.area=ar.area where a.transaccion='35' and a.folio like '%ALM%' and pedido<>'' and c.linea_producto not in ('10','15','16','17','19','20','21', '22' ,'23', '8') and a.fecha >= @INI and a.fecha <= @FIN	and cantidad > 0 and d.cod_estab  = '" + LALM(CBALM.SelectedIndex) + "'"
        DGV.DataSource = LLENATABLAIF(QUERY, CADENAZ, DTDE.Value.Date, DTHASTA.Value.Date.AddDays(1))
        CONZ.Close()
        DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        DGV.Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells

        DGV.Columns(4).DefaultCellStyle = FORMATONUMERICO()
        CHECATABLA()

    End Sub
    Private Sub CHECATABLA()
        LBLRES.Text = DGV.Rows.Count.ToString + " registros encontrados"
    End Sub

    Private Sub IMPRIMIR()
        If CBALM.SelectedIndex = 0 Then
            MessageBox.Show("Favor de seleccionar un almacén", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            If CONZ.State = ConnectionState.Closed Or CONZ.State = ConnectionState.Broken Then
                CONZ.Open()
            End If
            CONZ.Open()
        Catch ex As Exception

        End Try

        Dim QUERY As String
        QUERY = "select a.fecha, a.folio, b.pedido, g.pedido_establecimiento, a.cod_prod, c.descripcion, cast((round(a.cantidad,0)) as int)CANTIDAD, a.cod_estab, d.nombre, g.notas, c.area,ar.nombre AREARESP,inicio=@INI, fin=dateadd(dd,-1,@FIN),otrocampo='' from dbo.entysal (nolock) as a inner join dbo.movimientos_internos (nolock) as b on b.folio=a.folio inner join dbo.productos (nolock) as c on c.cod_prod=a.cod_prod inner join dbo.establecimientos (nolock) as d on d.cod_estab=b.cod_estab_alterno left join dbo.pedestab (nolock) as g on g.folio=b.pedido inner join Areas (nolock)  ar on c.area=ar.area where a.transaccion='35' and a.folio like '%ALM%' and pedido<>'' and c.linea_producto not in ('10','15','16','17','19','20','21', '22' ,'23', '8') and a.fecha >= @INI and a.fecha <= @FIN	and cantidad > 0 and d.cod_estab  = '" + LALM(CBALM.SelectedIndex) + "'"
        ' DGV.DataSource = LLENATABLAIF(QUERY, CADENAZ, DTDE.Value.Date, DTHASTA.Value.Date.AddDays(1))
        Dim REP As New rptAlmacenCedis
        MOSTRARREPORTE(REP, "Listado de Entregas", LLENATABLAIF(QUERY, CADENAZ, DTDE.Value.Date, DTHASTA.Value.Date.AddDays(1)), "")
        CONZ.Close()
    End Sub

    Private Sub BTNIMPRIMIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNIMPRIMIR.Click
        IMPRIMIR()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        CARGADATOS()
    End Sub
End Class