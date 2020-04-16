/// <reference path="jquery-3.4.1.min.js" />
/// <reference path="axios.min.js" />
$(function () {
    axios.defaults.timeout = 20000
    axios.interceptors.request.use(
        config => {
            return config;
        },
        err => {
            return Promise.reject(err);
        }
    );

    // http response 拦截器
    axios.interceptors.response.use(
        response => {
            return response;
        },
        error => {
            // 超时请求处理
            var originalRequest = error.config;
            if (error.code == 'ECONNABORTED' && error.message.indexOf('timeout') != -1 && !originalRequest._retry) {
                originalRequest._retry = true
                return null;
            }

            if (error.response) {
                 // 403 授权过期
                if (error.response.status == 401) {
                    layer.msg('登录已失效，请重新登录！', {
                        offset: '15px'
                        , icon: 5
                    });
                    window.location.href = "/Login/Login"
                }
                // 403 无权限
                if (error.response.status == 403) {
                    layer.msg('请求失败，您无权限！', {
                        offset: '15px'
                        , icon: 5
                    });
                }
                // 500 异常
                if (error.response.status == 500) {
                    layer.msg('请求异常！', {
                        offset: '15px'
                        , icon: 5
                    });
                }
            }
            return ""; // 返回接口返回的错误信息
        }
    );
})


