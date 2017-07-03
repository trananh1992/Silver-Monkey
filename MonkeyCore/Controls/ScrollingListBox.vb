﻿Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Namespace Controls
    Public Class ScrollingListBox
        Inherits ListBox

#Region "Private Fields"

        Private Const SB_ENDSCROLL As Integer = 8
        Private Const SB_LEFT As Integer = 6
        Private Const SB_LINEDOWN As Integer = 1
        Private Const SB_LINELEFT As Integer = 0
        Private Const SB_LINERIGHT As Integer = 1
        Private Const SB_LINEUP As Integer = 0
        Private Const SB_PAGEBOTTOM As Integer = 7
        Private Const SB_PAGEDOWN As Integer = 3
        Private Const SB_PAGELEFT As Integer = 2
        Private Const SB_PAGERIGHT As Integer = 3
        Private Const SB_PAGETOP As Integer = 6
        Private Const SB_PAGEUP As Integer = 2
        Private Const SB_RIGHT As Integer = 7
        Private Const SB_THUMBPOSITION As Integer = 4
        Private Const SB_THUMBTRACK As Integer = 5
        Private Const SIF_ALL As Integer = SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS
        Private Const SIF_PAGE As Integer = &H2
        Private Const SIF_POS As Integer = &H4
        Private Const SIF_RANGE As Integer = &H1
        Private Const SIF_TRACKPOS As Integer = &H10
        Private Const WM_HSCROLL As Integer = &H114
        Private Const WM_VSCROLL As Integer = &H115

#End Region

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
            InitializeComponent()
            Me.DoubleBuffered = True
        End Sub

#End Region

#Region "Public Events"

        Public Event OnHorizontalScroll As ScrollEventHandler
        Public Event OnVerticalScroll As ScrollEventHandler

#End Region

#Region "Protected Methods"

        Protected Overrides Sub WndProc(ByRef msg As System.Windows.Forms.Message)
            If msg.Msg = WM_HSCROLL Then
                'If OnHorizontalScroll IsNot Nothing Then
                Dim si As New ScrollInfoStruct()
                si.fMask = SIF_ALL
                si.cbSize = Marshal.SizeOf(si)
                GetScrollInfo(msg.HWnd, 0, si)
                If msg.WParam.ToInt32() = SB_ENDSCROLL Then
                    Dim sargs As New ScrollEventArgs(ScrollEventType.EndScroll, si.nPos)
                    RaiseEvent OnHorizontalScroll(Me, sargs)
                End If
                'End If
            End If
            If msg.Msg = WM_VSCROLL Then
                'If OnVerticalScroll IsNot Nothing Then
                Dim si As New ScrollInfoStruct()
                si.fMask = SIF_ALL
                si.cbSize = Marshal.SizeOf(si)
                GetScrollInfo(msg.HWnd, 0, si)
                If msg.WParam.ToInt32() = SB_ENDSCROLL Then
                    Dim sargs As New ScrollEventArgs(ScrollEventType.EndScroll, si.nPos)
                    RaiseEvent OnVerticalScroll(Me, sargs)
                End If
                'End If
            End If
            MyBase.WndProc(msg)
        End Sub

#End Region

#Region "Private Methods"

        Private Shared Function GetScrollInfo(hWnd As IntPtr, n As Integer, ByRef lpScrollInfo As ScrollInfoStruct) As Integer
        End Function

        Private Sub InitializeComponent()
            Me.SuspendLayout()
            '
            ' scrolled
            '
            Me.Size = New System.Drawing.Size(120, 95)
            Me.ResumeLayout(False)
        End Sub

#End Region

#Region "Private Structs"

        Private Structure ScrollInfoStruct

#Region "Public Fields"

            Public cbSize As Integer
            Public fMask As Integer
            Public nMax As Integer
            Public nMin As Integer
            Public nPage As Integer
            Public nPos As Integer
            Public nTrackPos As Integer

#End Region

        End Structure

#End Region

    End Class
End Namespace