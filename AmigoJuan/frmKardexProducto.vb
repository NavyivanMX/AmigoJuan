Public Class frmKardexProducto
    Dim GRU, PRO As String
    Dim LALM As New List(Of String)
    Dim DT As New DataTable
    Private Sub frmKardexProducto_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)
        If Not MODULOGENERAL.LLENACOMBOBOX(CBALM, Me.LALM, String.Concat(New String() {"SELECT A.CLAVE,A.NOMBRE FROM USUARIOSALMACENES D INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE INNER JOIN EMPRESASGASTOS E ON A.EMPRESA=E.CLAVE WHERE D.USUARIO='", frmPrincipal.Usuario, "' AND A.ACTIVO=1 AND E.GRUPO='", frmPrincipal.Empresa, "' ORDER BY A.NOMBRE"}), frmPrincipal.CadenaConexion) Then
            MessageBox.Show("Usted no cuenta con Almacenes asignados para visualizar este reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Me.Close()
        End If
        GRU = ""
        PRO = ""
    End Sub
    Private Sub CHECATABLA()
        'Dim TOT As Double
        'TOT = 0
        'Dim X, COL As Integer
        'If RB2.Checked Then
        '    COL = 7
        'Else
        '    COL = 5
        'End If

        'For X = 0 To DGV.Rows.Count - 1
        '    TOT += DGV.Item(COL, X).Value
        'Next
        'LBLVT.Text = "Venta Total: " + FormatNumber(TOT, 2).ToString
    End Sub

    Private Sub CARGADATOS()
        If frmPrincipal.CHECACONX Then
            Cursor.Show()
            Cursor.Current = Cursors.WaitCursor
            Dim qUERY As String = String.Concat(New String() {"SELECT D.FECHA,Movimiento='Entrada',D.EU Cantidad,D.EC [Costo Unitario],D.ET [Costo Total],D.IU [Inv Cantidad],D.IC [Costo Promedio],D.IT [Total Inventario], L.NOMBRECORTO LOTE, D.OBSERVACION,P.NOMBRE PRODUCTO,D.FECHAMOV FROM KARDEXPRODUCTO D INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE WHERE D.EU>0 AND D.ALMACEN ='", Me.LALM.Item(Me.CBALM.SelectedIndex), "' AND D.FECHA>=@INI AND D.FECHA<=@FIN AND P.CLAVE='", Me.PRO, "'"})
            qUERY = String.Concat(New String() {qUERY, " UNION ALL SELECT D.FECHA,Movimiento='Salida',D.SU Cantidad,D.SC [Costo Unitario],D.ST [Costo Total],D.IU [Inv Cantidad],D.IC [Costo Promedio], D.IT [Total Inventario], L.NOMBRECORTO LOTE, D.OBSERVACION,P.NOMBRE PRODUCTO,D.FECHAMOV FROM KARDEXPRODUCTO D INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE INNER JOIN LOTESP L ON D.PRODUCTO=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE WHERE D.SU>0 AND D.ALMACEN ='", Me.LALM.Item(Me.CBALM.SelectedIndex), "' AND D.FECHA>=@INI AND D.FECHA<=@FIN  AND P.CLAVE='", Me.PRO, "' ORDER BY L.NOMBRECORTO,FECHA"})
            DT = MODULOGENERAL.LLENATABLAIF(qUERY, frmPrincipal.CadenaConexion, Me.DTDE.Value.Date, Me.DTHASTA.Value.Date.AddDays(1))
            Me.DGV.DataSource = DT
            Me.DGV.Columns.Item(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            'Me.CHECATABLA()
            DGV.Columns(11).Visible = False
            DGV.Columns(10).Visible = False
            DGV.Columns(7).Visible = False
            DGV.Columns(6).Visible = False

            DGV.Columns(2).DefaultCellStyle = FORMATONUMERICO()
            DGV.Columns(3).DefaultCellStyle = FORMATONUMERICO()
            DGV.Columns(4).DefaultCellStyle = FORMATONUMERICO()
            DGV.Columns(5).DefaultCellStyle = FORMATONUMERICO()
            DGV.Columns(6).DefaultCellStyle = FORMATONUMERICO()
            DGV.Columns(7).DefaultCellStyle = FORMATONUMERICO()

            DGV.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(7).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DGV.Columns(9).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub BTNMOSTRAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNMOSTRAR.Click
        CARGADATOS()
    End Sub
    Private Sub IMPRIMIRREGISTRO()
        Dim str3 As String
        Dim str4 As String = ""
        Dim str2 As String = ""
        Dim str As String = Me.DGV.Item(8, Me.DGV.CurrentRow.Index).Value.ToString
        Dim length As Integer = str.Length
        If (length > 14) Then
            If (str.Substring(0, 8) = "TRASPASO") Then
                str4 = "TRASPASO"
            ElseIf (str.Substring(0, 14) = "Salida Diversa") Then
                str4 = "SALIDA"
            ElseIf (str.Substring(0, 15) = "Entrada Diversa") Then
                str4 = "ENTRADA"
            Else
                str4 = "SIN IDENTIFICAR"
            End If
        End If
        If (str4 = "SALIDA") Then
            str2 = str.Substring(15, (length - 15))
            str2.TrimEnd(New Char() {" "c})
            str2.TrimStart(New Char() {" "c})
            str3 = String.Concat(New String() {"SELECT A.NOMBRE ALMACEN,E.NOSALIDA NOORDEN,C.NOMBRE CONCEPTO,AR.NOMBRE AREA,E.FECHA,US.NOMBRE USUARIO,P.NOMBRE PRODUCTO,SUM(D.CANTIDAD)CANTIDAD,U.NOMBRE UNIDAD,SUM(D.CANTIDAD*D.COSTOPROMEDIO) TOTAL,E.OBSERVACIONES FROM SALIDASALMACENES E INNER JOIN ALMACENES A ON E.ALMACEN=A.CLAVE INNER JOIN SALIDASALMACENESLOTES D ON E.ALMACEN=D.ALMACEN AND E.NOSALIDA=D.NOSALIDA INNER JOIN MOVIMIENTOSINVENTARIO C ON E.CONCEPTO=C.CLAVE INNER JOIN AREASSALIDAS AR ON E.AREA=AR.CLAVE INNER JOIN LOTESPRODUCTOS L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE AND L.GRUPO=P.GRUPO INNER JOIN UNIDADES U ON P.UNIDAD=U.CLAVE INNER JOIN USUARIOSINV US ON E.USUARIO=US.USUARIO WHERE E.ALMACEN='", Me.LALM.Item(Me.CBALM.SelectedIndex), "' AND E.NOSALIDA='", str2, "' GROUP BY A.NOMBRE,E.NOSALIDA,C.NOMBRE,AR.NOMBRE,E.FECHA,US.NOMBRE,P.NOMBRE,U.NOMBRE,E.OBSERVACIONES"})
            Dim rEP As New rptSalidaDiversa
            MODULOGENERAL.MOSTRARREPORTE(rEP, ("Salida Diversa: " & str2), MODULOGENERAL.LLENATABLA(str3, frmPrincipal.CadenaConexion), "")
        End If
        If (str4 = "ENTRADA") Then
            str2 = str.Substring(&H10, (length - &H10))
            str2.TrimEnd(New Char() {" "c})
            str2.TrimStart(New Char() {" "c})
            Dim diversa2 As New rptEntradaDiversa
            str3 = String.Concat(New String() {"SELECT A.NOMBRE ALMACEN,E.NOORDEN,C.NOMBRE CONCEPTO,AR.NOMBRE AREA,E.FECHA,US.NOMBRE USUARIO,P.NOMBRE PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.TOTAL,E.OBSERVACION OBSERVACIONES FROM ENTRADASALMACENES E INNER JOIN ALMACENES A ON E.ALMACEN=A.CLAVE INNER JOIN DETALLEENTRADASALMACENES D ON E.ALMACEN=D.ALMACEN AND E.NOORDEN=D.NOORDEN INNER JOIN MOVIMIENTOSINVENTARIO C ON E.CONCEPTO=C.CLAVE INNER JOIN AREASSALIDAS AR ON E.AREA=AR.CLAVE INNER JOIN LOTESPRODUCTOS L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE AND L.GRUPO=P.GRUPO INNER JOIN UNIDADES U ON D.UNIDAD=U.CLAVE INNER JOIN USUARIOSINV US ON E.USUARIO=US.USUARIO INNER JOIN GRUPOSARTICULOS G ON P.GRUPO=G.CLAVE WHERE E.ALMACEN='", Me.LALM.Item(Me.CBALM.SelectedIndex), "' AND E.NOORDEN='", str2, "' ORDER BY G.NOMBRE,P.NOMBRE"})
            MODULOGENERAL.MOSTRARREPORTE(diversa2, ("Entrada Diversa: " & str2), MODULOGENERAL.LLENATABLA(str3, frmPrincipal.CadenaConexion), "")
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        frmClsBusqueda.BUSCAR("SELECT CLAVE,Nombre,NOMBRECORTO,ACTIVO FROM PRODUCTOS", " WHERE NOMBRE", " ORDER BY NOMBRE", "Búsqueda de Productos", "Nombre del Producto", "Productos(s)", 2, frmPrincipal.CadenaConexion, False)
        If (frmClsBusqueda.DialogResult = DialogResult.Yes) Then
            Me.PRO = frmClsBusqueda.TREG.Rows.Item(0).Item(0).ToString()
            Me.LBLNP.Text = frmClsBusqueda.TREG.Rows.Item(0).Item(1).ToString()
        End If
    End Sub

    Private Sub DGV_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV.CellDoubleClick
        IMPRIMIRREGISTRO()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If DT.Rows.Count <= 0 Then
            MessageBox.Show("Favor de primero cargar la información", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If
        ExportarExcel(DT, "Kardex de Producto " + LBLNP.Text, True)
    End Sub

    Private Sub BTNIMPRIMIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNIMPRIMIR.Click
        If DT.Rows.Count <= 0 Then
            MessageBox.Show("Favor de primero cargar la información", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If
        Dim REP As New rptKardexProducto
        MOSTRARREPORTE(REP, "Kárdex de producto ", DT, "")
    End Sub
End Class