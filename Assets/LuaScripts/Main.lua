--[[
    Title:
        Lua UI 框架

    Description:
        Lua程序入口

    Author : Zhaiyurong

    Date : 2022.2

    Modify:

]]

require 'Update'


UIManager = require 'UIManager'

local function Main()
    print("Main Func Run")
end

Main()


--监控_G
local mt = {}

mt.__index = function(t, k)
    print("正在调用不存在的全局变量--", k)
    return rawget(t, k)
end

mt.__newindex = function(t, k, v)
    print("此处定义了全局变量--", k, debug.traceback())
    rawset(t, k, v)
end

setmetatable(_G, mt)