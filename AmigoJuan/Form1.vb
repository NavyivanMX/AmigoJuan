Public Class frmPrincipal
    Public CONX As New SqlClient.SqlConnection
    Public CadenaConexion As String
    Public SucursalBase As String
    Public Serie As String
    Public NivelBase, Tipo As Integer
    Public NombreSucursal As String
    Public Empresa As String
    Public NombreComun As String
    Public Usuario As String
    Public Ciudad As String
    Public PagoTarjeta As Boolean
    Public Ajuste As Boolean
    Public CorteXX As Boolean
    Public RIFA, RIFA2 As Boolean
    Public LCopias As New List(Of Integer)
    Dim FrmsHijos(25) As System.Windows.Forms.Form
    Dim VERSION As String
    Dim LIGA As String
    Public NombreUsuario As String
    Public IP As String
    Public IPFE As String
    Public BancoCuenta As String
    Public LTIC As New List(Of String)
    Dim RES As Boolean
    Public VCFD As String
    Public CA As New clsActualizacion
    Public Sistema, EmisorBase As String
    Public AplicaBodega As Boolean
    Public Direccion As String
    Dim LM As New List(Of ToolStripMenuItem)
    Dim LSM As New List(Of ToolStripMenuItem)
    Dim LSSM As New List(Of ToolStripMenuItem)
    Public Perfil As Integer
    Public FacturaLibre As Boolean
    Public Resguardo As Boolean
    Public VentaSinResguardo As Integer
    Public DT As DataTable
    Private Sub frmPrincipal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Ciudad = "Los Mochis, Sin."
        VERSION = "BETA 1.17"
        VCFD = "3.3"
        FacturaLibre = True
        Resguardo = False
        VentaSinResguardo = 0
        EmisorBase = ""
        'IP = "201.120.24.238"
        IP = "navyserver.ddns.net"
        IPFE = "navyserver.ddns.net"
        'If PRUEBAPING("BAHAMUT", "Ip Local", False) Then
        '    IP = "192.168.2.75"
        '    IPFE = "192.168.2.75"
        'End If
        'PBFONDO.Controls.Add(PBCNC)
        'PBFONDO.Controls.Add(Label3)
        'PBFONDO.Controls.Add(Label4)
        'PBFONDO.Controls.Add(Label2)
        'PBFONDO.Controls.Add(Label6)
        Sistema = "POS PM"
        VISUALIZACION(Me)
        SB.Items(0).Text = "Los Mochis, Sin. Hoy es " + FormatDateTime(Now, DateFormat.LongDate) + " "
        SB.Items(3).Text = "Versión: " + VERSION
        'CadenaConexion = "Data Source=MINIME-PC;Initial Catalog=POSCAVA;Integrated Security=True"
        CadenaConexion = "Data Source=" + IP + ",1433;Network Library=DBMSSOCN;Initial Catalog=COMARJA;User ID=dbaadmin;Password=Xoporte1234."
        CONX.ConnectionString = CadenaConexion

        If Not CONECTAR("WWW.BAJASUN.COM.MX") Then
            frmAyuda.Show()
            frmCambioIP.ShowDialog()
            If frmCambioIP.DialogResult = Windows.Forms.DialogResult.Yes Then
                If Not CHECACONX() Then
                    Dim VAYU As New frmAyuda
                    VAYU.Show()
                    MessageBox.Show("No se puede Conectar con el servidor, intente en un momento por favor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Me.Close()
                Else
                    INICIAR()
                End If
            Else
                Me.Close()
            End If
        Else
            INICIAR()
            'For i As Integer = 0 To PrinterSettings.InstalledPrinters.Count - 1 '' ciclo de todas las impresoras k tengo instalada en la pc
            '    Dim a As New PrinterSettings() ''' variable de propiedades de impresora
            '    a.PrinterName = PrinterSettings.InstalledPrinters(i).ToString() '' la propiedad de impresora PrinterName= a la impresoras instalada en la posicion i
            '    MessageBox.Show(a.PrinterName)
            '    'If a.PrinterName.ToUpper = NOMBREIMPRESORA.ToUpper Then '' si el nombre de la impresora instalada en la posicion i = a la impresora k le mando como parametro
            '    '    Return PrinterSettings.InstalledPrinters(i).ToString() '' nombre de impresora=a la impresora instalada
            '    'End If
            'Next
            'Dim Impres1717Z
        End If
    End Sub
    Dim LPERFIL As New List(Of String)
    Private Sub INICIAR()
        If VERIFICAVERSION() Then
            If Not VERIFICAUBICACION() Then
                Me.Close()
                Exit Sub
            End If
            Dim VSES As New frmLogin
            VSES.ShowDialog()
            If VSES.DialogResult = Windows.Forms.DialogResult.Yes Then
                MessageBox.Show("Bienvenido al sistema de Inventarios " + NombreComun, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                If NivelBase >= 5 Then
                    ADMINISTRADOR(True)
                ElseIf NivelBase = 1 Then
                    ADMINISTRADOR(False)
                Else
                    MessageBox.Show("El usuario no tiene permitido ingresar al sistema", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                    Exit Sub
                End If
                'CARGASD(TPV, CHIE)
                'POSICIONAICONOS(TPV)
                'CARGAPROMOCIONES()me.
                'CAMBIAFONDO()
                'CARGARIFAS()
                'Try
                '    CA.COMPROBAR()
                'Catch ex As Exception
                '    MessageBox.Show(ex.Message)
                'End Try
            Else
                Me.Close()
            End If
        Else
            frmLigaVersion.MOSTRAR(LIGA, "C:\NUEVAVERSION\AmigoJuan.RAR", "Actualización Amigo Juan")
            Me.Close()
        End If
    End Sub
    Private Sub INICIALIZAMENU()
        Dim S2 As New ToolStripMenuItem
        Dim S3 As New ToolStripMenuItem
        Dim S6 As New ToolStripMenuItem
        Dim S7 As New ToolStripMenuItem
        Dim S13 As New ToolStripMenuItem

        Dim S14 As New ToolStripMenuItem
        Dim S15 As New ToolStripMenuItem
        Dim S16 As New ToolStripMenuItem
        Dim S17 As New ToolStripMenuItem
        Dim S18 As New ToolStripMenuItem
        Dim S19 As New ToolStripMenuItem
        Dim S10 As New ToolStripMenuItem
        Dim S11 As New ToolStripMenuItem
        Dim S12 As New ToolStripMenuItem

        Dim S20 As New ToolStripMenuItem
        Dim S21 As New ToolStripMenuItem
        Dim S22 As New ToolStripMenuItem
        Dim S23 As New ToolStripMenuItem
        Dim S24 As New ToolStripMenuItem
        Dim S25 As New ToolStripMenuItem
        Dim S26 As New ToolStripMenuItem
        Dim S27 As New ToolStripMenuItem
        Dim S28 As New ToolStripMenuItem
        Dim S29 As New ToolStripMenuItem
        Dim S30 As New ToolStripMenuItem
        Dim S31 As New ToolStripMenuItem
        Dim S32 As New ToolStripMenuItem
        Dim S33 As New ToolStripMenuItem
        Dim S34 As New ToolStripMenuItem
        Dim S35 As New ToolStripMenuItem
        Dim S36 As New ToolStripMenuItem
        Dim S37 As New ToolStripMenuItem
        Dim S38 As New ToolStripMenuItem
        Dim S39 As New ToolStripMenuItem
        Dim S40 As New ToolStripMenuItem
        Dim S41 As New ToolStripMenuItem
        Dim S42 As New ToolStripMenuItem
        Dim S43 As New ToolStripMenuItem
        Dim S44 As New ToolStripMenuItem
        Dim S45 As New ToolStripMenuItem
        Dim S46 As New ToolStripMenuItem
        Dim S47 As New ToolStripMenuItem
        Dim S48 As New ToolStripMenuItem
        Dim S49 As New ToolStripMenuItem
        Dim S50 As New ToolStripMenuItem
        Dim S51 As New ToolStripMenuItem
        Dim S52 As New ToolStripMenuItem
        Dim S53 As New ToolStripMenuItem
        Dim S54 As New ToolStripMenuItem
        Dim S55 As New ToolStripMenuItem
        Dim S56 As New ToolStripMenuItem
        Dim S57 As New ToolStripMenuItem
        Dim S58 As New ToolStripMenuItem
        Dim S59 As New ToolStripMenuItem
        Dim S60 As New ToolStripMenuItem
        Dim S61 As New ToolStripMenuItem
        Dim S62 As New ToolStripMenuItem
        Dim S63 As New ToolStripMenuItem
        Dim S64 As New ToolStripMenuItem
        Dim S65 As New ToolStripMenuItem
        Dim S66 As New ToolStripMenuItem
        Dim S67 As New ToolStripMenuItem
        Dim S68 As New ToolStripMenuItem
        Dim S69 As New ToolStripMenuItem
        Dim S70 As New ToolStripMenuItem

        Dim S5 As New ToolStripMenuItem
        Dim SS6 As New ToolStripMenuItem
        Dim SS7 As New ToolStripMenuItem
        Dim SS8 As New ToolStripMenuItem
        Dim SS9 As New ToolStripMenuItem

        LM.Clear()
        LSM.Clear()
        LSSM.Clear()
        LM.Add(MM1)
        LM.Add(MM2)
        LM.Add(MM3)
        LM.Add(MM4)
        'LM.Add(MM5)
        'LM.Add(MM6)
        'LM.Add(MM7)
        'LM.Add(MM8)
        'LM.Add(MM9)

        LSM.Add(S1)
        LSM.Add(S2)
        LSM.Add(S3)
        LSM.Add(S4)
        LSM.Add(S5)
        LSM.Add(S6)
        LSM.Add(S7)
        LSM.Add(S8)
        LSM.Add(S9)
        LSM.Add(S10)
        LSM.Add(S11)
        LSM.Add(S12)
        LSM.Add(S13)
        LSM.Add(S14)
        LSM.Add(S15)
        LSM.Add(S16)
        LSM.Add(S17)
        LSM.Add(S18)
        LSM.Add(S19)
        LSM.Add(S20)
        LSM.Add(S21)
        LSM.Add(S22)
        LSM.Add(S23)
        LSM.Add(S24)
        LSM.Add(S25)
        LSM.Add(S26)
        LSM.Add(S27)
        LSM.Add(S28)
        LSM.Add(S29)
        LSM.Add(S30)
        LSM.Add(S31)
        LSM.Add(S32)
        LSM.Add(S33)
        LSM.Add(S34)
        LSM.Add(S35)
        LSM.Add(S36)
        LSM.Add(S37)
        LSM.Add(S38)
        LSM.Add(S39)
        LSM.Add(S40)
        LSM.Add(S41)
        LSM.Add(S42)
        LSM.Add(S43)
        LSM.Add(S44)
        LSM.Add(S45)
        LSM.Add(S46)
        LSM.Add(S47)
        LSM.Add(S48)
        LSM.Add(S49)
        LSM.Add(S50)
        LSM.Add(S51)
        LSM.Add(S52)
        LSM.Add(S53)
        LSM.Add(S54)
        LSM.Add(S55)
        LSM.Add(S56)
        LSM.Add(S57)
        LSM.Add(S58)
        LSM.Add(S59)
        LSM.Add(S60)
        LSM.Add(S61)
        LSM.Add(S62)
        LSM.Add(S63)
        LSM.Add(S64)
        LSM.Add(S65)
        LSM.Add(S66)
        LSM.Add(S67)
        LSM.Add(S68)
        LSM.Add(S69)
        LSM.Add(S70)
        'LSM.Add(S71)
        'LSM.Add(S72)
        'LSM.Add(S73)
        'LSM.Add(S74)
        'LSM.Add(S75)
        'LSM.Add(S76)
        'LSM.Add(S77)
        'LSM.Add(S78)
        'LSM.Add(S79)

        Dim SS1 As New ToolStripMenuItem
        Dim SS2 As New ToolStripMenuItem
        Dim SS3 As New ToolStripMenuItem
        Dim SS4 As New ToolStripMenuItem
        Dim SS5 As New ToolStripMenuItem

        LSSM.Add(SS1)
        LSSM.Add(SS2)
        LSSM.Add(SS3)
        LSSM.Add(SS4)
        LSSM.Add(SS5)
        LSSM.Add(SS6)
        LSSM.Add(SS7)
        LSSM.Add(SS8)
        LSSM.Add(SS9)
        'LSSM.Add(SS10)
        'LSSM.Add(SS11)
        'LSSM.Add(SS12)
        'LSSM.Add(SS13)
        'LSSM.Add(SS14)
        'LSSM.Add(SS15)
        'LSSM.Add(SS16)
        'LSSM.Add(SS17)
        'LSSM.Add(SS18)
        'LSSM.Add(SS19)
        'LSSM.Add(SS20)
        'LSSM.Add(SS21)
        'LSSM.Add(SS22)
        'LSSM.Add(SS23)
        'LSSM.Add(SS24)
        'LSSM.Add(SS25)
        'LSSM.Add(SS26)
        'LSSM.Add(SS27)
        'LSSM.Add(SS28)
        'LSSM.Add(SS29)
        'LSSM.Add(SS30)
        'LSSM.Add(SS31)
        'LSSM.Add(SS32)
        'LSSM.Add(SS33)
        'LSSM.Add(SS34)
        'LSSM.Add(SS35)
        'LSSM.Add(SS36)
        'LSSM.Add(SS37)
        'LSSM.Add(SS38)
        'LSSM.Add(SS39)
        'LSSM.Add(SS40)
        'LSSM.Add(SS41)
        'LSSM.Add(SS42)
        'LSSM.Add(SS43)
        'LSSM.Add(SS44)
        'LSSM.Add(SS45)
        'LSSM.Add(SS46)
        'LSSM.Add(SS47)
        'LSSM.Add(SS48)


        Dim X As Integer
        For X = 0 To LM.Count - 1
            LM(X).Visible = False
        Next
        For X = 0 To LSM.Count - 1
            LSM(X).Visible = False
        Next
        For X = 0 To LSSM.Count - 1
            LSSM(X).Visible = False
        Next

    End Sub
    Private Sub CARGAPERFILES(ByVal vPERFIL As Integer)
        If Not CHECACONX() Then
            Exit Sub
        End If
        Exit Sub
        Perfil = vPERFIL
        INICIALIZAMENU()

        'PictureBox4.Visible = False



        Dim SQL As New SqlClient.SqlCommand("SELECT MENU FROM PERFILESD WHERE SISTEMA=1 AND PERFIL='" + Perfil.ToString + "'", CONX)
        Dim LEC As SqlClient.SqlDataReader
        LEC = SQL.ExecuteReader
        While LEC.Read
            Try
                LM(LEC(0) - 1).Visible = True
            Catch ex As Exception
            End Try
        End While
        LEC.Close()
        SQL.CommandText = "SELECT SUBMENU FROM PERFILESD WHERE SISTEMA=1 AND PERFIL='" + Perfil.ToString + "'"
        LEC = SQL.ExecuteReader
        While LEC.Read
            Try
                LSM(LEC(0) - 1).Visible = True
                'If (LEC(0)) = 23 Then
                '    PBF.Visible = True
                '    Label3.Visible = True
                'End If


            Catch ex As Exception
                ' MsgBox(ex.Message + LEC(0).ToString)
            End Try
        End While
        LEC.Close()
        SQL.CommandText = "SELECT SUBSUBMENU FROM PERFILESD WHERE SISTEMA=1 AND PERFIL='" + Perfil.ToString + "' AND SUBSUBMENU<>0"
        LEC = SQL.ExecuteReader
        While LEC.Read
            Try
                LSSM(LEC(0) - 1).Visible = True
                If (LEC(0)) = 1 Then
                    'PBF.Visible = True
                    'LBLENTRADAS.Visible = True
                End If
            Catch ex As Exception
                ' MsgBox(ex.Message + LEC(0).ToString)
            End Try
        End While
        LEC.Close()
        SQL.Dispose()

        'S13.Visible = False
        S12.Visible = False
        'S7.Visible = False
        'S6.Visible = False

        'S27.Visible = False
        'S28.Visible = False
        'S36.Visible = False
        'S52.Visible = False
        'S64.Visible = False
        'S59.Visible = False
        'S51.Visible = False
        'S67.Visible = False
        'MessageBox.Show(LSM(57).Visible.ToString, "", MessageBoxButtons.OK)
        'PBPRENOTAS.Visible = S58.Visible
        'PBCLI.Visible = S4.Visible
        'PBF.Visible = SS1.Visible
        'PictureBox1.Visible = S57.Visible
    End Sub
    Public Function CHECACONX() As Boolean
        If Me.CONX.State = ConnectionState.Closed Or Me.CONX.State = ConnectionState.Broken Then
            Try
                Me.CONX.Open()
            Catch ex As Exception
                MessageBox.Show("La conexión NO esta realizada, la informacion no se ha procesado, intente en un momento por favor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End Try
        End If
        Return True
    End Function
    Private Function CHECACONXX()
        If Me.CONX.State = ConnectionState.Closed Or Me.CONX.State = ConnectionState.Broken Then
            Try
                Me.CONX.Open()
            Catch ex As Exception
                Return False
            End Try
        End If
        Return True
    End Function
    Public Function CONECTAR(ByVal PAGINA As String)
        Try
            If CHECACONXX() Then
                Return True
            Else
                MessageBox.Show("Intento de conexión 1 fallido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MessageBox.Show("Intento de conexión 1 fallido (principal)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End Try
        CadenaConexion = "Data Source=201.120.180.23,1433;Network Library=DBMSSOCN;Initial Catalog=COMARJA;User ID=dbaadmin;Password=masterkey"
        CONX.ConnectionString = CadenaConexion
        Try
            CONX.Open()
            IP = "201.120.180.23"
            IPFE = "201.120.180.23"
            MessageBox.Show("Conectado desde .23", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return True
        Catch ex As Exception
            MessageBox.Show("Intento de conexión 2 fallido (Alterno A)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End Try
        CadenaConexion = "Data Source=201.120.180.232,1433;Network Library=DBMSSOCN;Initial Catalog=COMARJA;User ID=dbaadmin;Password=masterkey"
        CONX.ConnectionString = CadenaConexion
        Try
            CONX.Open()
            IP = "201.120.180.232"
            IPFE = "201.120.180.232"
            MessageBox.Show("Conectado desde 232", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return True
        Catch ex As Exception
            MessageBox.Show("Intento de conexión 3 fallido (Alterno B)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End Try
        IP = CA.DNSLookup(PAGINA)
        If IP = "" Then
            Return False
        Else
            CadenaConexion = "Data Source=" + IP + ",1433;Network Library=DBMSSOCN;Initial Catalog=COMARJA;User ID=dbaadmin;Password=masterkey"
            CONX.ConnectionString = CadenaConexion
            Try
                CONX.Open()
                MessageBox.Show("Conectado desde " + IP + "", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Return True
            Catch ex As Exception
                MessageBox.Show("Intento de conexión 4 fallido (Alterno No-IP)" + IP, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End Try
        End If
        Return False
    End Function
    Private Sub CreateShortCut(ByVal strLinkName_ As String, ByVal strTargetPath_ As String _
             , ByVal blnDesktop_ As Boolean, Optional ByVal strPath_ As String = "" _
             , Optional ByVal strArguments_ As String = "" _
             , Optional ByVal strDescription_ As String = "" _
             , Optional ByVal strHotKey_ As String = "" _
             , Optional ByVal strIconLocation_ As String = "" _
             , Optional ByVal strWorkingDirectory_ As String = "")

        Dim shell As Object = CreateObject("WScript.shell")
        Dim link As Object

        If blnDesktop_ Then
            Dim DesktopPath As Object = shell.SpecialFolders("Desktop")
            link = shell.CreateShortcut _
                   (DesktopPath & "\" & strLinkName_ & ".lnk")
        Else
            link = shell.CreateShortcut _
                   (strPath_ & "\" & strLinkName_ & ".lnk")
        End If
        Try
            With link
                ' Argumentos
                .Description = strLinkName_              ' Nombre del Acceso directo
                .TargetPath = strTargetPath_             ' Destino
                .WindowStyle = 1                         ' Ejecutar
                .Save()
            End With
        Catch ex As Exception

        End Try

    End Sub
    Private Function VERIFICAUBICACION() As Boolean
        Return True
        Dim FL As New System.IO.FileInfo(Application.ExecutablePath)
        Dim ORIGEN As String
        ORIGEN = FL.DirectoryName + "\AmigoJuan.exe"
        If FL.DirectoryName <> "C:\" Then
            MessageBox.Show("El archivo NO se encuentra en la unidad disco local C: Se creará una copia, favor de cerrar todas las aplicaciones", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Try
                System.IO.File.Delete("C:\AmigoJuan.exe")
            Catch ex As Exception

            End Try
            System.IO.File.Copy(ORIGEN, "C:\AmigoJuan.exe")
            Try
                System.IO.File.Delete(ORIGEN)
            Catch ex As Exception

            End Try
            CreateShortCut("Acceso Directo a Inventario Amigo Juan", "C:\AmigoJuan.exe", True)
            Return False
        End If
        Return True
    End Function

    Private Function LASERIE() As String
        Return ""
    End Function
    Private Sub ADMINISTRADOR(ByVal V As Boolean)
        Serie = LASERIE()
        Me.Text = "Sistema de Inventarios: " + NombreComun
        SB.Items(0).Text = Me.Ciudad + " Hoy es " + FormatDateTime(Now, DateFormat.LongDate) + " "
        'PBC.Enabled = V
        ''PBF.Enabled = V
        'PBAI.Enabled = V
        'PBM2.Enabled = V
        ''PBCLI.Enabled = V
        '' A1.Enabled = V
        'S2.Enabled = V
        'S3.Enabled = V
        'S5.Enabled = V
        'S8.Enabled = V
        'S9.Enabled = V
        'S10.Enabled = V
        'S11.Enabled = V
        'S4.Enabled = V
        ''A15.Enabled = V
        'S12.Enabled = V
        'S15.Enabled = V
        'S16.Enabled = V
        'S17.Enabled = V
        ' ''A11.Enabled = V
        ''A12.Enabled = V
        ''A14.Enabled = V

        'PBCN.Enabled = V
        'PBCNCRE.Enabled = V
        'PBAI.Enabled = V
        ''PBM2.Enabled = V
        'SS2.Enabled = V
        'SS4.Enabled = V
        'SS3.Enabled = V
        'M9.Enabled = V
        'M8.Enabled = V
        '' R7.Enabled = V
        'S34.Enabled = V
        'S68.Enabled = True
        'S37.Visible = False
        'S37.Enabled = V
        'S30.Enabled = AplicaBodega
        'S13.Enabled = AplicaBodega

        'If NivelBase = 10 Then
        '    PBAI.Enabled = True
        '    M9.Enabled = True
        '    S4.Enabled = True
        '    ' A15.Enabled = True
        '    S12.Enabled = True
        '    S15.Enabled = True
        '    S16.Enabled = True
        '    S17.Enabled = True
        '    S18.Enabled = True
        '    S19.Enabled = True
        '    S20.Enabled = True
        '    S21.Enabled = True
        '    S35.Enabled = True
        '    S36.Enabled = True

        'Else
        '    PBAI.Enabled = False
        '    M9.Enabled = False
        '    S4.Enabled = False
        '    ' A15.Enabled = False
        '    S12.Enabled = False
        '    S15.Enabled = False
        '    S16.Enabled = False
        '    S17.Enabled = False
        '    S18.Enabled = False
        '    S19.Enabled = False
        '    S20.Enabled = False
        '    S21.Enabled = False
        '    S35.Enabled = False
        '    S36.Enabled = False
        'End If


        'If V Then
        '    'Me.BackColor = Color.Brown
        '    'Dim locacion As String
        '    'locacion = "FONDOCAVA"
        '    'PBFONDO.ImageLocation = "C:/FOTOSTIENDA/" + locacion + ".JPG"
        '    Me.BackColor = Color.DeepSkyBlue
        'Else
        '    Me.BackColor = Color.DarkGray
        'End If
        'If SucursalBase = "PM" Then
        '    S8.Visible = True
        '    S9.Visible = True
        '    S10.Visible = True
        '    S11.Visible = True
        '    S12.Visible = True

        '    S30.Visible = True
        '    S31.Visible = True
        '    S32.Visible = True
        '    S34.Visible = True
        '    S58.Visible = True
        '    S37.Visible = True
        '    S68.Visible = False
        'Else
        '    S58.Visible = False
        '    S8.Visible = False
        '    S9.Visible = False
        '    S10.Visible = False
        '    'A4.Visible = False
        '    S12.Visible = False
        '    S68.Visible = True
        '    S30.Visible = False
        '    S31.Visible = False
        '    S32.Visible = False
        '    S34.Visible = False
        'End If
        CARGAPERFILES(Perfil)
    End Sub
    Public Sub CHECATRASDECO()
        CHECATRASPASO()
    End Sub

    Public Function CHECATRASPASO() As Boolean
        If Not Me.CHECACONX Then
            Return False
        End If
        Dim SQL As New SqlClient.SqlCommand("SELECT ORIGEN FROM TRASPASOS WHERE ORIGEN='" + Me.SucursalBase + "' AND ACEPTADO=0 AND RESUELTO=0", Me.CONX)
        Dim LEC As SqlClient.SqlDataReader
        LEC = SQL.ExecuteReader

        If LEC.Read Then
            LEC.Close()

            Return True
        End If
        LEC.Close()
        SQL.CommandText = "SELECT DESTINO FROM TRASPASOS WHERE DESTINO='" + Me.SucursalBase + "' AND ACEPTADO=1 AND RECIBIDO=0"
        LEC = SQL.ExecuteReader
        If LEC.Read Then
            LEC.Close()

            Return True
        End If
        LEC.Close()
        Return False
        Return False
    End Function
    Private Sub A1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S1.Click
        Dim VEM As New frmEmpresa
        VEM.ShowDialog()
    End Sub

    Private Sub A3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S9.Click
        Dim VGRU As New frmFamilias
        VGRU.ShowDialog()
    End Sub

    Private Sub A4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S11.Click
        Dim VPRO As New frmProductosF
        VPRO.ShowDialog()
    End Sub
    Private Function VERIFICAVERSION() As Boolean
        Try
            CHECACONX()
            'MessageBox.Show("desconecta", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Dim VER As String
            Dim SQL As New SqlClient.SqlCommand("SELECT VERSION,LIGA FROM VERSIONES WHERE SISTEMA='INV'", Me.CONX)
            Dim LEC As SqlClient.SqlDataReader
            LEC = SQL.ExecuteReader
            If LEC.Read Then
                VER = LEC(0)
                LIGA = LEC(1)
                LEC.Close()
                If VER = VERSION Then
                    Return True
                Else
                    Return False
                End If
            Else
                LEC.Close()
                Return False
            End If
        Catch ex As Exception
            'CE.Escribir(Sistema, Now, "Verificar Version", ex.ToString)
            Return True
        End Try
    End Function

    Private Sub A8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S8.Click
        Dim VUNI As New frmUnidades
        VUNI.ShowDialog()
    End Sub

    Private Sub A5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub A6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S14.Click
        Dim VPRO As New frmProveedores
        VPRO.ShowDialog()
    End Sub

    Private Sub A10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim VCONG As New frmConceptosGastos
        VCONG.ShowDialog()
    End Sub

    Private Sub A11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim VSUB As New frmSubConceptosGastos
        VSUB.ShowDialog()
    End Sub

    Private Sub A12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim VCSUB As New frmConceptosSubConceptosGastos
        VCSUB.ShowDialog()
    End Sub

    Private Sub M1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub



    Private Sub ClientesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S5.Click
        Dim VCLI As New frmClientes
        VCLI.ShowDialog()
    End Sub

    Private Sub ClienteRFCToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub PBFONDO_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim TAMA As System.Drawing.Size
        TAMA = Me.Size
        TAMA.Height = TAMA.Height - 100
        TAMA.Width = TAMA.Width - 10
        PBCNC.Size = TAMA
        PBCNC.SizeMode = PictureBoxSizeMode.CenterImage
    End Sub

    Private Sub M4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S24.Click
        Dim VEA As New frmEntradaDiversa
        VEA.ShowDialog()
    End Sub

    Private Sub A7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S4.Click
        Dim VUSU As New frmUsuarios
        VUSU.ShowDialog()
    End Sub

    Private Sub A9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S12.Click
        Dim VEQUI As New frmEquivalenciasProductos
        VEQUI.ShowDialog()
    End Sub


    Private Sub AyudaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AyudaToolStripMenuItem.Click
        Dim VAYU As New frmAyuda
        VAYU.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        SB.Items(1).Text = "Comercializadora de Mariscos Amigo Juan " + Format(Now, "hh:mm:ss tt")
    End Sub

    Private Sub S10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S10.Click
        Dim VDEP As New frmDepartamentos
        VDEP.ShowDialog()
    End Sub





    Private Sub S26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S26.Click
        Dim VSA As New frmSalidaDiversa
        VSA.ShowDialog()
    End Sub

    'Private Sub PBC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PBC.Click
    '    Dim VSA2 As New frmSalidaDiversa
    '    VSA2.ShowDialog()
    'End Sub

    Private Sub PBU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PBU.Click
        Dim VSES As New frmLogin
        VSES.ShowDialog()
        If VSES.DialogResult = Windows.Forms.DialogResult.Yes Then
            If NivelBase >= 5 Then
                ADMINISTRADOR(True)
            Else
                ADMINISTRADOR(False)
            End If
            'LBLTI.Text = NombreSucursal
        Else
            Me.Close()
        End If
    End Sub

    Private Sub EntradasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EntradasToolStripMenuItem.Click
        Dim VRED As New frmReporteEntradaDiversa
        VRED.ShowDialog()
    End Sub

    Private Sub SalidasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalidasToolStripMenuItem.Click
        Dim VRSD As New frmReporteSalidaDiversa
        VRSD.ShowDialog()
    End Sub

    Private Sub TemporadasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub S43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S43.Click
        Dim VRINV As New frmReporteInventario
        VRINV.ShowDialog()
    End Sub

    Private Sub KardexProductoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KardexProductoToolStripMenuItem.Click
        Dim VKP As New frmKardexProducto
        VKP.ShowDialog()
    End Sub

    Private Sub TipoDeProductoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TipoDeProductoToolStripMenuItem.Click
        Dim VTP As New frmTipoProducto
        VTP.ShowDialog()
    End Sub

    Private Sub PBCLI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PBCLI.Click
        Dim VRI As New frmReporteInventario
        VRI.ShowDialog()
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub PBF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PBF.Click
        Dim VED As New frmEntradaDiversa
        VED.ShowDialog()
    End Sub

    Private Sub PBC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PBC.Click
        Dim VSD As New frmSalidaDiversa
        VSD.ShowDialog()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Dim VRAC As New frmReporteAlmacenCedis
        VRAC.ShowDialog()
    End Sub
End Class
