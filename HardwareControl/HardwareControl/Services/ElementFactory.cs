using System;
using System.Collections.Generic;
using HardwareControl.Elements;
using HardwareControl.Services;

namespace HardwareControl
{
    class ElementFactory
    {
        private Dictionary<ElementsType, IElementCreator> _dictionary;
        private static int _elements = 0;

        #region Singleton

        private static ElementFactory _sharedFactory = null;

        public static ElementFactory SharedFactory
        {
            get
            {
                if (_sharedFactory == null)
                {
                    _sharedFactory = new ElementFactory();
                }
                return _sharedFactory;
            }
        }

        #endregion

        private ElementFactory()
        {
            _dictionary = new Dictionary<ElementsType, IElementCreator>();

            _dictionary.Add(ElementsType.And, new ElementCreator<AndElement>());
            _dictionary.Add(ElementsType.Input, new ElementCreator<InputElement>());
            _dictionary.Add(ElementsType.Not, new ElementCreator<NotElement>());
            _dictionary.Add(ElementsType.NotAnd, new ElementCreator<NotAndElement>());
            _dictionary.Add(ElementsType.NotOr, new ElementCreator<NotOrElement>());
            _dictionary.Add(ElementsType.Or, new ElementCreator<OrElement>());
            _dictionary.Add(ElementsType.NotXor, new ElementCreator<NotXorElement>());
            _dictionary.Add(ElementsType.Output, new ElementCreator<OutputElement>());
        }

        public Element CreateElement(ElementsType type, IOController controller)
        {
            Element tmp = _dictionary[type].Create();
            tmp.SetName("Element" + _elements.ToString());
            _elements++;
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
            Element tmp = _dictionary[type].Create();
            tmp.SetName(name);
            _elements++;
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
