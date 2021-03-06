﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Demo.YuriOnIce.Relationship.ViewModel;
using DragAndDropMVVM.Behavior;
using DragAndDropMVVM.Controls;

namespace Demo.YuriOnIce.Relationship.Controls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Demo.YuriOnIce.Relationship.Controls"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Demo.YuriOnIce.Relationship.Controls;assembly=Demo.YuriOnIce.Relationship.Controls"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CharacterDiagram/>
    ///
    /// </summary>
    public class CharacterDiagram : ConnectionDiagramBase
    {
        static CharacterDiagram()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CharacterDiagram), new FrameworkPropertyMetadata(typeof(CharacterDiagram)));
        }

        public CharacterDiagram():base()
        {
             DataContext = new DiagramViewModel();
        }

        bool _isEnd = false;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);


            BehaviorCollection bhcol = Interaction.GetBehaviors(this);
            bhcol.Add(new DiagramElementDragBehavior());
            bhcol.Add(new DrawLineDragBehavior());
            bhcol.Add(new DrawLineDropBehavior());
        }


        public override Point CenterPosition
        {
            get
            {
                //if(ArrivalLines.Any() || DepartureLines.Any())
                //{

                //    return new Point(base.CenterPosition.X, base.CenterPosition.Y + 30);

                //}

                //return new Point(base.CenterPosition.X, base.CenterPosition.Y + 20); ;

                return new Point(base.CenterPosition.X, base.CenterPosition.Y +  (_isEnd ? 20 : 30)); 
            }
        }

        public override string DiagramUUID
        {
            get
            {
                return ((DiagramViewModel)DataContext).Index.ToString();
            }

            set
            {
               // Can't set the ActionComment
            }
        }


        protected override void OnDrop(DragEventArgs e)
        {

            _isEnd = true;

            base.OnDrop(e);
        }

        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            _isEnd = false;
            base.OnPreviewDragOver(e);
        }
    }
}
