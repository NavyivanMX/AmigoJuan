Imports System.Data.SqlClient

Public Class frmProductosF
    Dim LFAM As New List(Of String)
    Dim LDEP As New List(Of String)
    Dim LESP As New List(Of String)
    Dim LCAL As New List(Of String)
    Dim LMED As New List(Of String)
    Dim LTPRO As New List(Of String)
    Dim LGCC As New List(Of String)
    Dim LGIEPS As New List(Of String)
    Dim LSIM As New List(Of String)
    Dim LGINV As New List(Of String)
    Dim LGO As New List(Of String)
    Dim LGV As New List(Of String)
    Dim LGT As New List(Of String)
    Dim LUV As New List(Of String)
    Dim LUGEN As New List(Of String)
    Dim LUI As New List(Of String)
    Dim LUCC As New List(Of String)
    Dim LUC As New List(Of String)
    Dim LCED As New List(Of String)
    Dim LAS As New List(Of String)
    Dim LMP As New List(Of String)
    Dim LBT As New List(Of String)
    Dim LGP As New List(Of String)
    Dim LPRE As New List(Of String)
    Dim LCLA As New List(Of String)
    Dim LNOM As New List(Of String)
    Dim LTI As New List(Of String)
    Dim LALM1 As New List(Of String)
    Dim LALM2 As New List(Of String)
    Dim LALM3 As New List(Of String)
    Dim LTA As New List(Of String)
    Dim LGTP As New List(Of String)
    Dim IND As Integer

    Dim LFIVA As New List(Of String)
    Dim LFIEPS As New List(Of String)
    Dim LPYSSAT As New List(Of String)
    Private Sub frmProductosF_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim engine As New SkinEngine
        'MyProject.Forms.frmPrincipal.SENGINE.AddForm(Me)
        'Me.Refresh()
        Me.Icon = frmPrincipal.Icon
        VISUALIZACION(Me)

        LLENA2LISTAS(LCLA, LNOM, "SELECT CLAVE,NOMBRE FROM PRODUCTOS ORDER BY NOMBRE", frmPrincipal.CadenaConexion)
        IND = 0
        CHECAIND()
        LLENACOMBOBOX2(CBFAM, LFAM, "SELECT CLAVE,NOMBRE FROM FAMILIAS WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
        LLENACOMBOBOX2(CBCC, LGCC, "SELECT CLAVE,NOMBRE FROM CCPRODUCTOS WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
        LLENACOMBOBOX2(CBUCIC, LUCC, "SELECT CLAVE,NOMBRE FROM UNIDADES WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
        LLENACOMBOBOX2(CBUINV, LUI, "SELECT CLAVE,NOMBRE FROM UNIDADES WHERE ACTIVO=1 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
                
        LimpiarForm(Controls)

        LLENACOMBOBOX2(CBFIVA, LFIVA, "SELECT CLAVE,NOMBRE FROM CSATTIPOFACTOR WHERE CLAVE<>2 ORDER BY NOMBRE", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")
        LLENACOMBOBOX2(CBFIEPS, LFIEPS, "SELECT CLAVE,NOMBRE FROM CSATTIPOFACTOR ORDER BY NOMBRE ", frmPrincipal.CadenaConexion, "Favor de seleccionar", "")

        ACTIVAR(True)
        LIMPIAR()
    End Sub
    Private Sub ACTIVAR(ByVal V As Boolean)
        Me.TXTCLA.Enabled = V
        Me.TXTCP.Enabled = Not V
        Me.TXTCON.Enabled = Not V
        Me.TXTNOM.Enabled = Not V
        Me.TXTDES.Enabled = Not V
        Me.TXTNC.Enabled = Not V
        Me.TXTNI.Enabled = Not V
        Me.CBFAM.Enabled = Not V
        Me.CBDEPTO.Enabled = Not V
        Me.CBESP.Enabled = Not V
        Me.CBCAL.Enabled = Not V
        Me.CBMED.Enabled = Not V
        Me.CBTP.Enabled = Not V
        Me.CBCC.Enabled = Not V
        TXTTIVA.Enabled = Not V
        Me.BTNMOSTRAR.Enabled = Not V
      
        Me.CBACT.Enabled = Not V
      
        Me.CBTP.Enabled = Not V
      
        Me.CBUINV.Enabled = Not V
        Me.CBUCIC.Enabled = Not V
      
        Me.TXTDES.Enabled = Not V

        'Me.DGV3.Enabled = Not V
        Me.TXTCP.Enabled = Not V

        Me.BTNGUARDAR.Enabled = Not V
        CBFIEPS.Enabled = Not V
        CBFIVA.Enabled = Not V
        TXTCPYSSAT.Enabled = Not V

        TXTCOSTO.Enabled = Not V
    End Sub

    Private Sub CARGADATOS()
        If frmPrincipal.CHECACONX Then
            Me.ACTIVAR(False)
            DEFAULTS()
            Dim command As New SqlCommand("SELECT CP,CONSECUTIVO,NOMBRE,NOMBRECORTO,DESCRIPCION,IMAGEN,FAMILIA,DEPARTAMENTO,ESPECIE,CALIDAD,MEDIDA,TIPOPRODUCTO,GRUPOCC,GRUPOIEPS,TASAIEPS,ACTIVO,UNIDADINV,UNIDADCICLO,FACTORIVA,FACTORIEPS,TASAIVA,CPYSSAT,COSTO,ORDEN FROM PRODUCTOS WHERE CLAVE=@CLA", frmPrincipal.CONX)
            command.Parameters.Add("@CLA", SqlDbType.Int).Value = Me.TXTCLA.Text
            Dim reader As SqlDataReader = command.ExecuteReader
            If reader.Read Then
                TXTCP.Text = reader.Item(0).ToString
                TXTCON.Text = reader.Item(1).ToString
                TXTNOM.Text = reader.Item(2).ToString
                TXTDES.Text = reader.Item(3).ToString
                TXTNC.Text = reader.Item(4).ToString
                TXTNI.Text = reader.Item(5).ToString

                MODULOGENERAL.CARGAX(Me.LFAM, CBFAM, reader.Item(6).ToString)
                MODULOGENERAL.CARGAX(LDEP, CBDEPTO, reader.Item(7).ToString)
                MODULOGENERAL.CARGAX(LESP, CBESP, reader.Item(8).ToString)
                MODULOGENERAL.CARGAX(LCAL, CBCAL, reader.Item(9).ToString)
                MODULOGENERAL.CARGAX(LMED, CBMED, reader.Item(10).ToString)
                MODULOGENERAL.CARGAX(LTPRO, CBTP, reader.Item(11).ToString)
                MODULOGENERAL.CARGAX(LGCC, CBCC, reader.Item(12).ToString)
                TXTTCIEPS.Text = reader.Item(14).ToString

                If CType(reader.Item(15), Boolean) Then
                    Me.CBACT.SelectedIndex = 1
                Else
                    Me.CBACT.SelectedIndex = 0
                End If


                CARGAX(LUI, CBUINV, reader.Item(16).ToString)
                CARGAX(LUCC, CBUCIC, reader.Item(17).ToString)

                'If CType(reader.Item(45), Boolean) Then

                CARGAX(LFIVA, CBFIVA, reader(18))
                CARGAX(LFIEPS, CBFIEPS, reader(19))
                TXTTIVA.Text = reader(20)

                TXTCPYSSAT.Text = reader.Item(21)
                TXTCOSTO.Text = reader.Item(22).ToString
                TXTORDEN.Text = reader.Item(23).ToString

           
            End If
            reader.Close()
            command.Dispose()

        End If
    End Sub

    Private Sub CHECAIND()
        If (Me.IND <= 0) Then
            Me.BTNANTG.Enabled = False
        Else
            Me.BTNANTG.Enabled = True
        End If
        If (Me.IND > (Me.LCLA.Count - 1)) Then
            Me.BTNSIGG.Enabled = False
        Else
            Me.BTNSIGG.Enabled = True
        End If
    End Sub


    Private Sub GUARDAR()
        If frmPrincipal.CHECACONX Then
            Dim num As Integer
            Dim str As String = Me.LFAM.Item(Me.CBFAM.SelectedIndex)
            Dim str2 As String = Me.LDEP.Item(Me.CBDEPTO.SelectedIndex)
            Dim str3 As String = Me.LESP.Item(Me.CBESP.SelectedIndex)
            Dim str4 As String = Me.LCAL.Item(Me.CBCAL.SelectedIndex)
            Dim str5 As String = Me.LMED.Item(Me.CBMED.SelectedIndex)
            Dim str6 As String = Me.LTPRO.Item(Me.CBTP.SelectedIndex)

            Dim str9 As String = Me.LTPRO.Item(Me.CBTP.SelectedIndex)

            Dim str14 As String = Me.LUI.Item(Me.CBUINV.SelectedIndex)
            Dim str15 As String = Me.LUCC.Item(Me.CBUCIC.SelectedIndex)
            Dim command4 As New SqlCommand With { _
                .Connection = frmPrincipal.CONX, _
                .CommandType = CommandType.StoredProcedure _
            }
            command4.Parameters.Add("@CLA", SqlDbType.Int).Value = TXTCLA.Text
            command4.Parameters.Add("@CP", SqlDbType.VarChar).Value = TXTCP.Text
            command4.Parameters.Add("@CON", SqlDbType.VarChar).Value = TXTCON.Text
            command4.Parameters.Add("@NOM", SqlDbType.VarChar).Value = TXTNOM.Text
            command4.Parameters.Add("@NOMC", SqlDbType.VarChar).Value = TXTNC.Text
            command4.Parameters.Add("@DES", SqlDbType.VarChar).Value = TXTDES.Text
            command4.Parameters.Add("@IMG", SqlDbType.VarChar).Value = TXTNI.Text
            command4.Parameters.Add("@FAM", SqlDbType.Int).Value = LFAM(CBFAM.SelectedIndex)
            command4.Parameters.Add("@DEP", SqlDbType.Int).Value = 0
            command4.Parameters.Add("@ESP", SqlDbType.Int).Value = 0
            command4.Parameters.Add("@CAL", SqlDbType.Int).Value = 0
            command4.Parameters.Add("@MED", SqlDbType.Int).Value = 0
            command4.Parameters.Add("@TP", SqlDbType.Int).Value = LTPRO(CBTP.SelectedIndex)
            command4.Parameters.Add("@GCC", SqlDbType.Int).Value = LGCC(CBCC.SelectedIndex)
            command4.Parameters.Add("@GIEPS", SqlDbType.Int).Value = 0 ' Me.LGIEPS.Item(Me.CBIEPS.SelectedIndex)

            'command4.Parameters.Add("@IEPS", SqlDbType.Float).Value = Me.TXTTASA.Text

            command4.Parameters.Add("@ACT", SqlDbType.Bit).Value = CBACT.SelectedIndex
            command4.Parameters.Add("@UINV", SqlDbType.Int).Value = LUI(CBUINV.SelectedIndex)
            command4.Parameters.Add("@UCIC", SqlDbType.Int).Value = LUCC(CBUCIC.SelectedIndex)

            command4.Parameters.Add("@FIVA", SqlDbType.Int).Value = LFIVA(CBFIVA.SelectedIndex)
            command4.Parameters.Add("@FIEPS", SqlDbType.Int).Value = LFIEPS(CBFIEPS.SelectedIndex)

            If TXTTIVA.Enabled Then
                command4.Parameters.Add("@TIVA", SqlDbType.Float).Value = TXTTIVA.Text
            Else
                command4.Parameters.Add("@TIVA", SqlDbType.Float).Value = 0
            End If
            If TXTTCIEPS.Enabled Then
                command4.Parameters.Add("@IEPS", SqlDbType.Float).Value = TXTTCIEPS.Text
            Else
                command4.Parameters.Add("@IEPS", SqlDbType.Float).Value = 0
            End If

            command4.Parameters.Add("@PYSSAT", SqlDbType.VarChar).Value = TXTCPYSSAT.Text
            command4.Parameters.Add("@COSTO", SqlDbType.Float).Value = TXTCOSTO.Text
            command4.Parameters.Add("@ORDEN", SqlDbType.Int).Value = TXTORDEN.Text
            command4.CommandText = "SPPRODUCTOSFV2"
            command4.ExecuteNonQuery()        
            MessageBox.Show("La Información ha sido Guardada correctamente", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.LIMPIAR()
            Me.ACTIVAR(True)
        End If
    End Sub

    Private Sub LIMPIAR()
        MODULOGENERAL.LimpiarForm(Me.Controls)

        DEFAULTS()
    End Sub
    Private Sub DEFAULTS()
        TXTCOSTO.Text = "0"
        TXTNI.Text = "."
        CBFIVA.SelectedIndex = 2
        CBFIEPS.SelectedIndex = 2
        TXTTCIEPS.Text = "0"
        TXTTIVA.Text = "0"
        TXTCPYSSAT.Text = "50121612"
        CARGAX(LUI, CBUINV, "1")
        CARGAX(LUCC, CBUCIC, "1")
        CBCC.SelectedIndex = 1
        CBACT.SelectedIndex = 1
        TXTORDEN.Text = 999
    End Sub


    Private Function CONSECUTIVO() As String
        Dim str2 As String = "NA"
        Dim command As New SqlCommand("SELECT DBO.SIGPRODUCTO()", frmPrincipal.CONX)
        Dim reader As SqlDataReader = command.ExecuteReader
        If reader.Read Then
            str2 = reader.Item(0).ToString
        End If
        reader.Close()
        command.Dispose()
        Me.TXTCLA.Text = str2
        Return str2
    End Function
    Private Sub BTNAGREGAR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNAGREGAR.Click
        Me.ACTIVAR(False)
        Me.CONSECUTIVO()
    End Sub

    Private Sub BTNBUSCAR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNBUSCAR.Click
        frmClsBusqueda.BUSCAR("SELECT P.CLAVE CLAVE,P.NOMBRE,P.ORDEN ,P.ACTIVO FROM PRODUCTOS P", " WHERE P.NOMBRE", " ORDER BY P.NOMBRE", "Búsqueda de Productos", "Nombre del Producto", "Producto(s)", 1, frmPrincipal.CadenaConexion, False)
        If (frmClsBusqueda.DialogResult = DialogResult.Yes) Then
            Me.TXTCLA.Text = frmClsBusqueda.TREG.Rows.Item(0).Item(0).ToString
            Me.CARGADATOS()
        End If
    End Sub

    Private Sub BTNGUARDAR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNGUARDAR.Click
        If ((((Me.TXTNOM.Text = "") Or (Me.TXTDES.Text = "")))) Then
            MessageBox.Show("Falta ingresar datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            'Dim num2 As Double
            'Try
            '    num2 = CType(Me.TXTTASA.Text, Double)
            'Catch exception1 As Exception
            '    Dim exception As Exception = exception1
            '    MessageBox.Show("IEPS No Válida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            '    Return
            'End Try

            Dim ABC As Double
            If TXTTIVA.Enabled Then
                Try
                    ABC = CType(TXTTIVA.Text, Double)
                    If ABC < 0 Or ABC > 0.16 Then
                        MessageBox.Show("Tasa Iva No Válida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                        Exit Sub
                    End If
                Catch ex As Exception
                    MessageBox.Show("Tasa Iva No Válida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                    Exit Sub
                End Try
            End If

            If TXTTCIEPS.Enabled Then
                Try
                    ABC = CType(TXTTCIEPS.Text, Double)
                    If ABC < 0 Or ABC > 43.77 Then
                        MessageBox.Show("Tasa IEPS No Válida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                        Exit Sub
                    End If
                Catch ex As Exception
                    MessageBox.Show("Tasa IEPS No Válida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                    Exit Sub
                End Try
            End If
            'If CBFAM.SelectedIndex = 0 Or CBCAL.SelectedIndex = 0 Or CBDEPTO.SelectedIndex = 0 Or CBESP.SelectedIndex = 0 Or CBMED.SelectedIndex = 0 Or CBTP.SelectedIndex = 0 Then
            If CBFAM.SelectedIndex = 0 Or CBTP.SelectedIndex = 0 Then
                MessageBox.Show("Favor de seleccionar clasificadores", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Exit Sub
            End If

            Me.GUARDAR()
        End If
    End Sub

    Private Sub CBFIVA_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CBFIVA.SelectedIndexChanged
        TXTTIVA.Enabled = False
        If CBFIVA.Enabled Then
            If LFIVA(CBFIVA.SelectedIndex) = "1" Then
                TXTTIVA.Enabled = True
            End If
        End If
     
    End Sub

    Private Sub CBFIEPS_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CBFIEPS.SelectedIndexChanged
        TXTTCIEPS.Enabled = False
        If LFIEPS(CBFIEPS.SelectedIndex) = "1" Or LFIEPS(CBFIEPS.SelectedIndex) = "2" Then
            TXTTCIEPS.Enabled = True
        End If
    End Sub

    Private Sub BTNANTG_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNANTG.Click
        Me.IND -= 1
        If (Me.IND >= 0) Then
            Me.TXTCLA.Text = Me.LCLA.Item(Me.IND)
            Me.CARGADATOS()
        End If
        Me.CHECAIND()
    End Sub

    Private Sub BTNSIGG_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNSIGG.Click
        Me.IND += 1
        If (Me.IND <= (Me.LCLA.Count - 1)) Then
            Me.TXTCLA.Text = Me.LCLA.Item(Me.IND)
            Me.CARGADATOS()
        End If
        Me.CHECAIND()
    End Sub

    Private Sub CBFAM_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CBFAM.SelectedIndexChanged
        MODULOGENERAL.LLENACOMBOBOX2(CBDEPTO, Me.LDEP, ("SELECT CLAVE,NOMBRE FROM DEPARTAMENTOS WHERE ACTIVO=1 AND FAMILIA='" & Me.LFAM.Item(Me.CBFAM.SelectedIndex) & "' ORDER BY NOMBRE"), frmPrincipal.CadenaConexion, "Favor de Seleccionar", "")
        MODULOGENERAL.LLENACOMBOBOX2(CBESP, Me.LESP, ("SELECT CLAVE,NOMBRE FROM ESPECIES WHERE ACTIVO=1 AND FAMILIA='" & Me.LFAM.Item(Me.CBFAM.SelectedIndex) & "' ORDER BY NOMBRE"), frmPrincipal.CadenaConexion, "Favor de Seleccionar", "")
        MODULOGENERAL.LLENACOMBOBOX2(CBMED, Me.LMED, ("SELECT CLAVE,NOMBRE FROM MEDIDAS WHERE ACTIVO=1 AND FAMILIA='" & Me.LFAM.Item(Me.CBFAM.SelectedIndex) & "' ORDER BY NOMBRE"), frmPrincipal.CadenaConexion, "Favor de Seleccionar", "")
        MODULOGENERAL.LLENACOMBOBOX2(CBCAL, Me.LCAL, ("SELECT CLAVE,NOMBRE FROM CALIDAD WHERE ACTIVO=1 AND FAMILIA='" & Me.LFAM.Item(Me.CBFAM.SelectedIndex) & "' ORDER BY NOMBRE"), frmPrincipal.CadenaConexion, "Favor de Seleccionar", "")
        MODULOGENERAL.LLENACOMBOBOX2(CBTP, Me.LTPRO, ("SELECT CLAVE,NOMBRE FROM TIPOPRODUCTO WHERE ACTIVO=1 AND FAMILIA='" & Me.LFAM.Item(Me.CBFAM.SelectedIndex) & "' ORDER BY NOMBRE"), frmPrincipal.CadenaConexion, "Favor de Seleccionar", "")
    End Sub

    Private Sub CBDEPTO_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CBDEPTO.SelectedIndexChanged

    End Sub

    Private Sub BTNCANCELAR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNCANCELAR.Click
        Me.ACTIVAR(True)
        Me.LIMPIAR()
    End Sub

    Private Sub LBLPYSSAT_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles LBLPYSSAT.LinkClicked
        System.Diagnostics.Process.Start("http://200.57.3.46:443/PyS/catPyS.aspx")
    End Sub

    Private Sub TXTCLA_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TXTCLA.KeyPress

        If e.KeyChar = Chr(13) Then
            If String.IsNullOrEmpty(TXTCLA.Text) Then
            Else
                CARGADATOS()
            End If
        End If

    End Sub

    Private Sub TXTNOM_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TXTNOM.TextChanged
        Me.LBLNP3.Text = Me.TXTNOM.Text
        Me.LBLNP3.Text = Me.TXTNC.Text
    End Sub

    Private Sub BTNBUSPRO_Click(ByVal sender As Object, ByVal e As EventArgs)
        frmClsBusqueda.BUSCAR("SELECT P.CLAVE CLAVE,P.NOMBRE,P.ACTIVO FROM PRODUCTOS P", " WHERE P.NOMBRE", " ORDER BY P.NOMBRE", "Búsqueda de Productos", "Nombre del Producto", "Producto(s)", 1, frmPrincipal.CadenaConexion, False)
        If (frmClsBusqueda.DialogResult = DialogResult.Yes) Then
  
        End If
    End Sub

    Private Sub BTNMOSTRAR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTNMOSTRAR.Click
        Dim VEQUI As New frmEquivalenciasProductos
        VEQUI.MOSTRAR(LFAM(CBFAM.SelectedIndex), TXTCLA.Text)
    End Sub

    Private Sub CBGV_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

    End Sub
End Class