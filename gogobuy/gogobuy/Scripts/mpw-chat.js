$(document).ready(function (mpwchat) {

    mpwchat.log = mpwchat.log || {};    //登入帳號

    mpwchat.log.write = function (message) {
        //顯示登入訊息
        if (typeof (message) != typeof ("")) message = "[kb] Invalid Log Message Format!";
        //疊加顯示新訊息
        var msg = document.getElementById("txaChat").value;
        msg += message + '\n';
        document.getElementById("txaChat").value = msg;
    }

    var conn = $.hubConnection();   //連結hub server
    var gHub = conn.createHubProxy('chatHub'); //建立hub server代理
    var joinedChat = false; //加入聊天的預設旗幟


    //加入連線的按鈕功能
    $('#btnJoin').click(function () {

        //聊天室上線
        if (!joinedChat) {
            document.getElementById("txaChat").value = ""; //Reset chat box
            mpwchat.log.write("[GOGOBUY Chat] 連線中...");
            conn.stop();
            //Start connection
            conn.start().done(function () {
                try {
                    mpwchat.log.write("[GOGOBUY Chat] 連線到伺服器");
                    //請求上線
                    var userName = document.getElementById("txbUserName").value;
                    if (!userName) {
                        mpwchat.log.write("[GOGOBUY Chat]{Error} 錯誤的使用者名稱");
                    } else {
                        mpwchat.log.write("[GOGOBUY Chat] 歡迎 " + userName + " 加入");
                        gHub.invoke('userInformation', userName)
                            .done(function () {
                                //請求送出
                            })
                            .fail(function (error) {
                                //請求失敗
                                mpwchat.log.write("[GOGOBUY Chat]{Error} " + error.message);
                                conn.stop();
                                joinedChat = false;
                            });
                    }
                } catch (ex) {
                    mpwchat.log.write("[GOGOBUY Chat]{Error} " + ex.message);
                    conn.stop();
                    joinedChat = false;
                }
            });

            //斷開連線
        } else {
            conn.stop();
        }
    });


    //送出訊息的按鈕功能
    $('#btnSend').click(function () {
        if (joinedChat) {
            //if (document.getElementById("txbMessage").value.length > 0) {
            gHub.invoke('messageFromUser', document.getElementById("txbMessage").value)
                .done(function () {
                    //Request Sent
                    document.getElementById("txbMessage").value = "";
                })
                .fail(function (error) {
                    //Request Failed
                    mpwchat.log.write("[GOGOBUY Chat]{Error} " + error.message);
                    conn.stop();
                    joinedChat = false;
                });
            //}
        }
    });

    //伺服器訊息: 使用者狀態
    gHub.on('userInfoResults', function (results) {
        if (results) {
            mpwchat.log.write("聊天室開啟");
            document.getElementById("btnJoin").value = "Disconnect";
        }
        else {
            conn.stop();
        }
        joinedChat = results;
    });

    //Hub 事件: 斷線事件
    conn.disconnected(function () {
        mpwchat.log.write("已離線");
        document.getElementById("txaUsers").value = "";
        document.getElementById("btnJoin").value = "Join";
        joinedChat = false;
    });

    //伺服器訊息: 新使用者加入
    gHub.on('newUser', function (user) {
        document.getElementById("txaChat").value += "會員 '" + user + "' 加入了聊天\n";
    });

    //伺服器訊息: 加入聊天的帳號列表
    //gHub.on('userList', function (users, userCount) {
    //    try {
    //        var userList = document.getElementById("txaUsers");
    //        var list = "";
    //        for (var i = 0; i < userCount; i++) {
    //            list += users[i];
    //            list += '\n';
    //        }
    //        userList.value = list;
    //    } catch (ex) {
    //        mpwchat.log.write("[GOGOBUY Chat]{Error} " + ex.message);
    //    }
    //});

    //伺服器訊息: 使用者離開
    gHub.on('userLeft', function (user) {
        try {
            document.getElementById("txaChat").value += "會員 '" + user + "' 已經離線 \n";
        } catch (ex) {
            mpwchat.log.write("[GOGOBUY Chat]{Error} " + ex.message);
        }
    });

    //伺服器訊息: 使用者的訊息內容
    gHub.on('messageToUsers', function (username, message) {
        try {
            document.getElementById("txaChat").value += username + ": " + message + '\n';

        } catch (ex) {
            mpwchat.log.write("[GOGOBUY Chat]{Error} " + ex.message);
        }
    });

}(window.mpwchat = window.mpwchat || {}));