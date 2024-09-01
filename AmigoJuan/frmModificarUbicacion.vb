Public Class frmModificarUbicacion
    Dim VLOTE, VUBI, VCONTEO As String
    Dim VCOSTO As Double
    Private Sub frmModificarUbicacion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)
    End Sub
    Public Sub MOSTRAR(ByVal NCLOTE As String, ByVal NPRO As String, ByVal NUBI As String, ByVal NLOTE As String, ByVal COSTO As Double, ByVal CONTEO As String)
        LBLLOTE.Text = NCLOTE
        LBLPRO.Text = NPRO
        LBLUBI.Text = NUBI
        TXTUBI.Text = NUBI
        VLOTE = NLOTE
        LBLCOS.Text = COSTO.ToString
        LBLCONTEO.Text = CONTEO
        TXTCONTEO.Text = CONTEO
        TXTCOSTO.Text = COSTO
        Me.ShowDialog()
    End Sub

    Private Sub BTNGUARDAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNGUARDAR.Click
        Try
            VCOSTO = CType(TXTCOSTO.Text, Double)
        Catch ex As Exception
            MessageBox.Show("Costo no especificado o erróneo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
        If VCOSTO < 0 Then
            MessageBox.Show("Costo no válido o erróneo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim num As Short = CShort(MessageBox.Show("¿Desea Guardar la Información?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        If (num = 6) Then
            Me.GUARDAR()
        End If
    End Sub
    Private Sub GUARDAR()
        If Not frmPrincipal.CHECACONX Then
            Exit Sub
        End If
        Dim SQL As New SqlClient.SqlCommand("UPDATE LOTESP SET UBICACION='" + TXTUBI.Text + "',COSTO='" + VCOSTO.ToString + "',RENDIMIENTO='" + TXTCONTEO.Text + "' WHERE CLAVE='" + VLOTE + "'", frmPrincipal.CONX)
        SQL.ExecuteNonQuery()
        MessageBox.Show("La información ha sido guardado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Me.DialogResult = Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub
End Class