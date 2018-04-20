﻿using System;

namespace Shop.AdminApi.Services
{
    public interface ICacheHelper
    {
        bool Exists(string key);
        T GetCache<T>(string key) where T : class;
        void SetCache(string key, object value);
        void SetCache(string key, object value, DateTimeOffset expiressAbsoulte);//设置绝对时间过期
        void RemoveCache(string key);
    }
}
