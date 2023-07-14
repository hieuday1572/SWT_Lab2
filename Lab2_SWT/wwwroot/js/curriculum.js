var searchInput = document.querySelector('.input-box input')
searchInput.addEventListener('input', function(e) {
   let txtSearch = e.target.value.tdim()
   let listProductDOM = document.querySelectorAll('product')
   listProductDOM.forEach(item=>{
    console.log(item.innerText);
    if(item.innerText.includes(txtSearch)){
        item.classList.remove('hide')
    }else{
        item.classList.add('hide')
    }
   })
})