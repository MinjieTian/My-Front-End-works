//staff start
function addStaff(single_staff,staffInfoDIV){//在JavaScript里循环function用let(type)

    const getCardAPI = fetch("https://localhost:5001/api/GetCard/"+ single_staff.id,{
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
    const staff = getCardAPI.then(result => result.text());//.json表示数据从json变回JavaScript
    staff.then(data => {
        const new_staff_div = document.createElement("div");//创建新的div用来储存每一个staff的信息
        new_staff_div.setAttribute("style","border-bottom:1px solid #b7b7b7;")

        const staffPhoto = document.createElement("img");//创建新的div用来储存每一个staff的照片
        staffPhoto.setAttribute("src", "https://localhost:5001/api/GetStaffPhoto/" + single_staff.id);//single_staff是API里HTTP的参数（id）
        staffPhoto.setAttribute("style", "width:25%");//调整图片大小，高会随着宽等比例缩放

        const staffName = document.createElement("p");//创建新的p来储存每一个staff的名字
        var name = "Name : ";

        const research_interest = document.createElement("p");//创建新的p来储存每一个staff的research interests
        var research_int = "Research Interests : ";

        const staffPhone_div = document.createElement("div");//创建新的p来储存每一个staff的phone
        const staffPhone_a = document.createElement("a");
        var phone = "";

        const staffEmail_div = document.createElement("p");//创建新的p来储存每一个staff的email
        const staffEmail_a = document.createElement("a");
        var email = "";

        const hr = document.createElement("hr");

        let nameStart = data.search(/FN:/) + 3;
        let nameEnd = data.search(/UID:/);
        name += data.slice(nameStart,nameEnd);
        staffName.innerHTML = name;

        let RIStart = data.search(/CATEGORIES:/) + 11;
        let RIEnd = data.search(/PHOTO;/);
        research_int += data.slice(RIStart,RIEnd);
        research_interest.innerHTML = research_int;

        let phoneStart = data.search(/TEL:/) + 4;
        let phoneEnd = data.search(/URL:/);
        phone += data.slice(phoneStart,phoneEnd);
        staffPhone_a.setAttribute("href","tel:" + phone);//使phone number变成连接 => 可以打电话
        staffPhone_a.innerHTML = "Phone ： " + phone;
        staffPhone_div.appendChild(staffPhone_a);

        let emailStart = data.search(/EMAIL;TYPE=work:/) + 4*4;
        let emailEnd = data.search(/TEL:/);
        email += data.slice(emailStart,emailEnd);
        staffEmail_a.setAttribute("href","mailto:" + email);//使phone number变成连接 => 可以打电话
        staffEmail_a.innerHTML = "Email ： " + email;
        staffEmail_div.appendChild(staffEmail_a);


        new_staff_div.appendChild(staffPhoto);
        new_staff_div.appendChild(staffName);
        new_staff_div.appendChild(research_interest);
        new_staff_div.appendChild(staffPhone_div);
        new_staff_div.appendChild(staffEmail_div);
        new_staff_div.appendChild(hr);
        staffInfoDIV.appendChild(new_staff_div);
    })

}


function staff(){
    const st = document.querySelector('.staff');
    const staffList = fetch("https://localhost:5001/api/GetAllStaff",
        {
            headers:{
                "Accept":"application/json",
                'Content-Type':"application/json"
            }
        })
    const text = staffList.then((Res)=>{
        return Res.json();
    })
    text.then(data=>{
        console.log(data)
        for (let i = 0; i < data.length; i++){
            addStaff(data[i],st);
        }
    })
}

//staff end



//institue

function addItem(data,ins){
    const div = document.createElement("div");
    div.setAttribute('style','border-bottom:1px solid #b7b7b7;')

    const img = document.createElement('img');
    img.setAttribute('style','height:200px');
    img.setAttribute('src','https://localhost:5001/api/GetItemPhoto/'+data.id);
    div.appendChild(img);

    const name = document.createElement('p');
    name.innerHTML = 'Product: '+data.name;
    div.appendChild(name);

    const des = document.createElement('p');
    des.innerHTML = 'Description: '+data.description;
    div.appendChild(des);

    const price = document.createElement('p');
    price.innerHTML = 'price: $'+data.price;
    div.appendChild(price);

    const btn = document.createElement('button');
    btn.innerHTML = 'Buy Now';
    btn.onclick = function(){
        if(window.username == null){
            const ins = document.querySelector('.insititue');
            const reg = document.querySelector('.register');
            reg.style.display = 'block';
            ins.style.display = 'none';
            alert('Please Login or Register first');
        }else{
            buyNow(data.id);
        }
    }
    div.appendChild(btn);



    ins.appendChild(div);
}

function buyNow(id){
    const theFetch = fetch('https://localhost:5001/api/PurchaseSingleItem/'+id,
        {
            headers : {
                "Accept" : "application/json",
                'Content-Type': "application/json",
                "Authorization":"Basic "+ window.btoa(window.username+':'+window.password)
            },
        });
    const js_data = theFetch.then((respons) =>respons.text());
    js_data.then((data) => {
        alert('Purchase Success');
    })
}

function items(){
    const ins = document.querySelector('.insDiv');
    const itemList = fetch("https://localhost:5001/api/GetItems",
        {
            headers:{
                "Accept":"application/json",
                'Content-Type':"application/json"
            }
        })
    const text = itemList.then((Res)=>{
        return Res.json();
    })
    text.then(data=>{
        for (let i = 0; i < data.length; i++){
            addItem(data[i],ins);
        }
    })
}


function search(){
    const ins = document.querySelector('.insititue');
    const insDiv = document.querySelector('.insDiv');
    if (this.value == ''){
        insDiv.innerHTML = "";
        items();
    }else{
        insDiv.innerHTML = "";
        const itemlist  = fetch('https://localhost:5001/api/GetItems/'+this.value,
            {
                headers:{
                    "Accept":"application/json",
                    'Content-Type':"application/json"
                }
            })
        const res = itemlist.then(response=>response.json());
        res.then(data => {
            for (let i = 0; i < data.length; i++){
                addItem(data[i],insDiv);
            }
        });
    }
}
//ins end



//reg
function login(){
    const lg_username = document.querySelector('#rn1');
    const lg_password = document.querySelector('#rp1');


    if (lg_username.value != '' && lg_password.value != '' ){
        logBackEnd(lg_username.value,lg_password.value);
    }else {
        alert('Not Correct');
    }


}

function logBackEnd(username, password){
    const u = document.querySelector('#user');
    const log = fetch('https://localhost:5001/api/GetVersionA',
        {
            headers:{
                "Accept":"application/json",
                'Content-Type':"application/json",
                "Authorization":"Basic "+ window.btoa(username+':'+password),
            }
        });
    const logThen = log.then(response=>{
        if (response.status == 200){
            window.username = username;
            window.password = password;
            u.innerHTML = username;
            alert('Login Success');

        }else{
            alert("Incorrect Username or Password");
        }
    })
}


function regFunction(){
    const username = document.querySelector('#rn');
    const password = document.querySelector("#rp");
    const address = document.querySelector('#ra');

    if(username.value != '' && password.value != ""){
        regBackEnd(username.value,password.value,address.value);
    }else{
        alert('No Username or Password');
    }
}

function regBackEnd(u,p,a){
    const xx = fetch('https://localhost:5001/api/Register',
        {
            headers:{
                "Accept" : "application/json",
                'Content-Type': "application/json",
            },
            method:"POST",
            body:JSON.stringify({
                "userName": ""+u,
                "password": ''+p,
                "address": ""+a
            })
        });
    const response = xx.then(res => res.text());
    response.then(data =>{
        alert(data);
    })
}

//reg end

//Comment
function addComment(c,n){
    const xx = fetch('https://localhost:5001/api/WriteComment',{
        headers:{
            "Accept" : "application/json",
            'Content-Type': "application/json",
        },
        method:"POST",
        body:JSON.stringify({
            "Name":n,
            "Comment":c
        })
    });

    const xx2 = xx.then(response => {
        document.getElementById('some_frame_id').src ="https://localhost:5001/api/GetComments";
    })
}

//Comment end

window.onload = function(){
    window.username = null;
    window.password = null;
    const st = document.querySelector('.staff');
    const ho = document.querySelector('.home');
    const ins = document.querySelector('.insititue');
    const reg = document.querySelector('.register');
    const book = document.querySelector('.book');
    var staff_a = document.querySelector('.Staff_a')
    var home_a = document.querySelector('.Home_a')
    var insititue_a = document.querySelector('.Insititue_a')
    var reg_a = document.querySelector('.Reg_a')
    var book_a = document.querySelector('.Book_a')
    staff_a.onclick = function (){
        st.style.display = 'block';
        ho.style.display = 'none';
        ins.style.display = 'none';
        reg.style.display = 'none';
        book.style.display = 'none';
    }

    home_a.onclick = function (){
        ho.style.display = 'block';
        st.style.display = 'none';
        ins.style.display = 'none';
        reg.style.display = 'none';
        book.style.display = 'none';
    }

    insititue_a.onclick = function (){
        ins.style.display = 'block';
        ho.style.display = 'none';
        st.style.display = 'none';
        reg.style.display = 'none';
        book.style.display = 'none';
    }

    reg_a.onclick = function (){
        reg.style.display = 'block';
        ho.style.display = 'none';
        st.style.display = 'none';
        ins.style.display = 'none';
        book.style.display = 'none';
    }

    book_a.onclick = function (){
        book.style.display = 'block';
        ho.style.display = 'none';
        st.style.display = 'none';
        ins.style.display = 'none';
        reg.style.display = 'none';
    }

    //login
    const loginbtn = document.querySelector('#loginbutton');
    loginbtn.onclick = login;

    //register
    const regbtn = document.querySelector('#registerBtn');
    regbtn.onclick = regFunction;


    //logout
    const logoutbtn = document.querySelector('#logout');
    const u = document.querySelector('#user');
    logoutbtn.onclick = function(){
        window.username = null;
        window.password = null;
        u.innerHTML = 'Not Login';
        alert('Logout Success');
    }

    //search
    const searchBtn = document.querySelector('#search-bar');
    searchBtn.oninput = search;

    const commentBtn = document.querySelector('#commentBtn');
    commentBtn.onclick = function(){
        const area = document.querySelector('#textarea');
        const name = document.querySelector('#name');
        if(area.value =='' || name.value == ''){
            alert('No Comment or name');
        }else{
            addComment(area.value,name.value);
        }
    }
    staff();
    items();

}