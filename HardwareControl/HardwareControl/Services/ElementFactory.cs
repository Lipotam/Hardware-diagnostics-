using System;
using System.Collections.Generic;

using HardwareControl.Elements;

namespace HardwareControl.Services
{
    class ElementFactory
    {
        private Dictionary<ElementsType, IElementCreator> _dictionary;
        private static int elements;

        #region Singleton

        private static ElementFactory sharedFactory;

        public static ElementFactory SharedFactory
        {
            get
            {
                if (sharedFactory == null)
                {
                    sharedFactory = new ElementFactory();
                }
                return sharedFactory;
            }
        }

        #endregion

        private ElementFactory()
        {
            this._dictionary = new Dictionary<ElementsType, IElementCreator>();

            this._dictionary.Add(ElementsType.And, new ElementCreator<AndElement>());
            this._dictionary.Add(ElementsType.Input, new ElementCreator<InputElement>());
            this._dictionary.Add(ElementsType.Not, new ElementCreator<NotElement>());
            this._dictionary.Add(ElementsType.NotAnd, new ElementCreator<NotAndElement>());
            this._dictionary.Add(ElementsType.NotOr, new ElementCreator<NotOrElement>());
            this._dictionary.Add(ElementsType.Or, new ElementCreator<OrElement>());
            this._dictionary.Add(ElementsType.NotXor, new ElementCreator<NotXorElement>());
            this._dictionary.Add(ElementsType.Output, new ElementCreator<OutputElement>());
        }

        public Element CreateElement(ElementsType type, IOController controller)
        {
            Element tmp = this._dictionary[type].Create();
            tmp.SetName("Element" + elements.ToString());
            elements++;
            if ((type == ElementsType.Input) || (type == ElementsType.Output))
            {
                controller.AddIOElement(tmp);
            }
            else
            {
                controller.AddElement(tmp);
            }
            return tmp;
        }

        public Element CreateElement(ElementsType type, IOController controller, String name)
        {
            Element tmp = this._dictionary[type].Create();
            tmp.SetName(name);
            elements++;
            if ((type == ElementsType.Input ) || (type == ElementsType.Output))
            {
                controller.AddIOElement(tmp);
            }
            else
            {
                controller.AddElement(tmp);
            }
            return tmp;
        }
    }
}
