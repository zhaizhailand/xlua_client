local TypeClass = require "common/TypeClass"
local BaseClass = {}

function BaseClass:New(...)
    local o = {}
    setmetatable(o, self)
    self.__index = self
    self:OnInit(...)
    return o
end

function BaseClass:OnInit(...)

end

function BaseClass:Define(className)
    local o = {}
    setmetatable(o, self)
    self.__index = self
    TypeClass.RegisterClass(className, o)
    return o
end

return BaseClass