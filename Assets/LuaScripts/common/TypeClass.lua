local _M = {}
local TypeClasses = {}

function _M.RegisterClass(type, class)
    print("Register new Class, type = ", type)
    TypeClasses[type] = class
end


return _M