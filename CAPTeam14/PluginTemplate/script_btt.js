// Lấy button ra.
var btn = document.getElementById("scroll-back");
// Khi người dùng cuộn xuống 20px từ đầu tài liệu, hãy hiển thị nút
window.onscroll = function(){
    scrollFunction();
}


function scrollFunction(){
    if(document.body.scrollTop > 20 || document.documentElement.scrollTop > 20){
        btn.style.display = "block";
    }
    else{
        btn.style.display = "none";
    }
}

function topFunction(){
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    document.body.style.transition = "all 1s linear";
}

btn.onclick = function(){
    topFunction()
}