Public Class frmReporteSalidaDiversa
    Dim LMOV As New List(Of String)
    Dim LCAT As New List(Of String)
    Dim LALM As New List(Of String)
    Dim DT As New DataTable
    Private Sub frmReporteSalidaDiversa_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)
        Dim cBALM As ComboBox = Me.CBALM
        Me.CBALM = cBALM
        If Not LLENACOMBOBOX(cBALM, LALM, "SELECT A.CLAVE, A.NOMBRE, A.NOMBRE  FROM USUARIOSALMACENES D INNER JOIN ALMACENES A ON D.ALMACEN=A.CLAVE INNER JOIN EMPRESASGASTOS E ON A.EMPRESA=E.CLAVE WHERE D.USUARIO='" + frmPrincipal.Usuario + "' AND A.ACTIVO=1 AND E.GRUPO='" + frmPrincipal.Empresa + "' ORDER BY A.NOMBRE", frmPrincipal.CadenaConexion) Then
            MessageBox.Show("Usted no cuenta con Almacenes asignados para visualizar este reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Me.Close()
        Else
            Dim cLBEMP As CheckedListBox = Me.CLBEMP
            MODULOGENERAL.LLENACLB(cLBEMP, Me.LMOV, "SELECT CLAVE,NOMBRE FROM MOVIMIENTOSINVENTARIO WHERE ORIENTACION=2 AND ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion)
            Me.CLBEMP = cLBEMP
            cLBEMP = Me.CLBG
            MODULOGENERAL.LLENACLB(cLBEMP, Me.LCAT, "SELECT CLAVE,NOMBRE FROM CLIENTES WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion)
            Me.CLBG = cLBEMP
        End If
    End Sub
    Private Function VERIFICAMARCADO() As Boolean
        If CLBEMP.CheckedItems.Count <= 0 Or CLBG.CheckedItems.Count <= 0 Then
            MessageBox.Show("Debe Seleccionar al menos una opción en cada sección", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return False
        End If
        Return True
    End Function
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
        If (Me.VERIFICAMARCADO AndAlso frmPrincipal.CHECACONX) Then
            Cursor.Show()
            Cursor.Current = Cursors.WaitCursor
            Dim qUERY As String = ("SELECT E.NOSALIDA,C.NOMBRE CONCEPTO,A.NOMBRE CLIENTE,E.FOLIO,L.NOMBRECORTO LOTE,P.NOMBRE PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,E.FECHA FROM SALIDASALMACENES E INNER JOIN DETALLESALIDASALMACENES D ON E.ALMACEN=D.ALMACEN AND E.NOSALIDA=D.NOSALIDA INNER JOIN MOVIMIENTOSINVENTARIO C ON E.CONCEPTO=C.CLAVE INNER JOIN CLIENTES A ON E.CLIENTE=A.CLAVE  INNER JOIN LOTESP L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 WHERE D.ALMACEN ='" & Me.LALM.Item(Me.CBALM.SelectedIndex) & "' AND E.FECHA>=@INI AND E.FECHA<=@FIN")
            Dim flag As Boolean = False
            qUERY = (qUERY & " AND (")
            Dim num3 As Integer = (Me.CLBEMP.Items.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num3)
                If Me.CLBEMP.GetItemChecked(i) Then
                    If flag Then
                        qUERY = (qUERY & " OR C.CLAVE='" & Me.LMOV.Item(i) & "'")
                    Else
                        qUERY = (qUERY & " C.CLAVE='" & Me.LMOV.Item(i) & "'")
                        flag = True
                    End If
                End If
                i += 1
            Loop
            qUERY = (qUERY & ")")
            flag = False
            flag = False
            qUERY = (qUERY & " AND (")
            Dim num4 As Integer = (Me.CLBG.Items.Count - 1)
            Dim j As Integer = 0
            Do While (j <= num4)
                If Me.CLBG.GetItemChecked(j) Then
                    If flag Then
                        qUERY = (qUERY & " OR E.CLIENTE='" & Me.LCAT.Item(j) & "'")
                    Else
                        qUERY = (qUERY & " E.CLIENTE='" & Me.LCAT.Item(j) & "'")
                        flag = True
                    End If
                End If
                j += 1
            Loop
            ''ORDER BY DBO.DIAMESAÑO(N.FECHA),N.FOLIO,P.NOMBRE,L.TEMPORADA,L.LOTE
            qUERY = (qUERY & ") ORDER BY DBO.DIAMESAÑO(E.FECHA),E.FOLIO,P.NOMBRE,L.TEMPORADA,L.LOTE")
            DT = MODULOGENERAL.LLENATABLAIF(qUERY, frmPrincipal.CadenaConexion, Me.DTDE.Value.Date, Me.DTHASTA.Value.Date.AddDays(1))
            Me.DGV.DataSource = DT
            Me.DGV.Columns.Item(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(7).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Me.DGV.Columns.Item(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            DGV.Columns(6).DefaultCellStyle = FORMATONUMERICOND(3)
            Me.CHECATABLA()
            Cursor.Current = Cursors.Default
        End If
    End Sub
    Private Sub CHECARCLB(ByVal IND As Integer, ByVal V As Boolean)
        Dim num As Integer
        If (IND = 1) Then
            Dim num2 As Integer = (Me.CLBEMP.Items.Count - 1)
            num = 0
            Do While (num <= num2)
                Me.CLBEMP.SetItemChecked(num, V)
                num += 1
            Loop
        End If
        If (IND = 5) Then
            Dim num3 As Integer = (Me.CLBG.Items.Count - 1)
            num = 0
            Do While (num <= num3)
                Me.CLBG.SetItemChecked(num, V)
                num += 1
            Loop
        End If
    End Sub
    Private Sub CHKESP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CHKESP.CheckedChanged, CHKPRO.CheckedChanged
        CHECARCLB(CType(sender.TAG, Integer), CType(sender, CheckBox).Checked)
    End Sub

    Private Sub BTNMOSTRAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNMOSTRAR.Click
        CARGADATOS()
    End Sub

    ''Actualizado
    Private Sub BTNIMPRIMIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNIMPRIMIR.Click
        If (Me.DGV.Rows.Count > 0) Then
            Dim QUERY As String
            QUERY = "SELECT A.NOMBRE ALMACEN,E.NOSALIDA,C.NOMBRE CONCEPTO,AREA='',E.FECHA,US.NOMBRE USUARIO,P.NOMBRECORTO PRODUCTO,D.CANTIDAD,U.NOMBRE UNIDAD,D.CANTIDAD*L.COSTO TOTAL,E.OBSERVACION,L.NOMBRECORTO,L.RENDIMIENTO CONTEO,L.UBICACION,CLI.NOMBRE CLIENTE,L.COSTO,E.FOLIO,E.FECHAREG FROM SALIDASALMACENES E INNER JOIN ALMACENES A ON E.ALMACEN=A.CLAVE INNER JOIN DETALLESALIDASALMACENES D ON E.ALMACEN=D.ALMACEN AND E.NOSALIDA=D.NOSALIDA INNER JOIN MOVIMIENTOSINVENTARIO C ON E.CONCEPTO=C.CLAVE  INNER JOIN LOTESP L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE INNER JOIN UNIDADES U ON U.CLAVE=1 INNER JOIN USUARIOS US ON E.USUARIO=US.USUARIO INNER JOIN CLIENTES CLI ON E.CLIENTE=CLI.CLAVE WHERE E.ALMACEN='" + LALM(CBALM.SelectedIndex) + "' AND E.NOSALIDA = " + DGV.Item(0, DGV.CurrentRow.Index).Value.ToString + " ORDER BY P.NOMBRE,L.TEMPORADA,L.LOTE"
            Dim rEP As New rptSalidaDiversa
            MODULOGENERAL.MOSTRARREPORTE(rEP, ("Salida Diversa: " & Me.DGV.Item(0, Me.DGV.CurrentRow.Index).Value.ToString), MODULOGENERAL.LLENATABLA(qUERY, frmPrincipal.CadenaConexion), "")
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If DT.Rows.Count <= 0 Then
            MessageBox.Show("Favor de primero cargar información", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ExportarExcel(DT, "Reporte Salidas Diversas " + DTDE.Value.Date.ToString("ddMMyyyy") + " hasta " + DTHASTA.Value.Date.ToString("ddMMyyyy"), True)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not VERIFICAMARCADO() Then
            Exit Sub
        End If
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If


        Dim QUERY, QC, QP, OBS As String
        QP = ""
        QC = ""

        OBS = "Desde: " + DTDE.Value.Date.ToShortDateString + " Hasta: " + DTHASTA.Value.Date.ToShortDateString
        If CHKESP.Checked Then
            OBS += " Todos los Conceptos."
        Else
            OBS += " Conceptos: "
            QC += " AND N.CONCEPTO IN ("
            Dim X As Integer
            For X = 0 To CLBEMP.Items.Count - 1
                If Me.CLBEMP.GetItemChecked(X) Then
                    OBS += CLBEMP.Items(X) + " & "
                    QC += LMOV.Item(X) + ","
                End If
            Next
            OBS = OBS.Remove(OBS.Length - 2, 1)
            QC = QC.Remove(QC.Length - 1, 1)
            QC += ")"
        End If
        If CHKPRO.Checked Then
            OBS += " Todos los Clientes."
        Else
            Dim X As Integer
            OBS += " Clientes: "
            QP += " AND N.CLIENTE IN ("
            For X = 0 To CLBG.Items.Count - 1
                If Me.CLBG.GetItemChecked(X) Then
                    OBS += CLBG.Items(X) + " & "
                    QP += LCAT.Item(X) + ","
                End If
            Next
            OBS = OBS.Remove(OBS.Length - 2, 1)
            QP = QP.Remove(QP.Length - 1, 1)
            QP += ")"
        End If
        Dim rEP As New rptGeneralSalidas
        QUERY = "SELECT A.NOMBRE ALMACEN,N.NOSALIDA,C.NOMBRE CONCEPTO,PRO.NOMBRE CLIENTE,D.CANTIDAD,L.NOMBRECORTO LOTE,P.NOMBRECORTO PRODUCTO,L.RENDIMIENTO CONTEO,L.COSTO,L.UBICACION,N.FECHA,OBSERVACIONES='" + OBS + "',N.FOLIO FROM SALIDASALMACENES N INNER JOIN DETALLESALIDASALMACENES D ON N.ALMACEN=D.ALMACEN AND N.NOSALIDA =D.NOSALIDA INNER JOIN ALMACENES A ON N.ALMACEN=A.CLAVE INNER JOIN MOVIMIENTOSINVENTARIO C ON N.CONCEPTO =C.CLAVE INNER JOIN CLIENTES PRO ON N.CLIENTE=PRO.CLAVE INNER JOIN LOTESP L ON D.LOTE=L.CLAVE INNER JOIN PRODUCTOS P ON L.PRODUCTO=P.CLAVE WHERE N.FECHA>=@INI AND N.FECHA<=@FIN"
        QUERY += QP + QC + " ORDER BY DBO.DIAMESAÑO(N.FECHA),N.FOLIO,N.NOSALIDA,P.NOMBRE,L.TEMPORADA,L.LOTE"
        MOSTRARREPORTE(rEP, "Reporte general de salidas ", LLENATABLAIF(QUERY, frmPrincipal.CadenaConexion, DTDE.Value.Date, DTHASTA.Value.Date.AddDays(1)), "")

    End Sub
End Class