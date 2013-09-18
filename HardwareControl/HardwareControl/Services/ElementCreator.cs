using HardwareControl.Elements;

namespace HardwareControl.Services
{
    class ElementCreator<element> : IElementCreator where element : Element, new()
    {
        public Element Create()
        {
            return new element();
        }
    }
}
