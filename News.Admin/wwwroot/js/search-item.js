
const searchInput = document.getElementById('searchInput');

searchInput.addEventListener('input', function () {
    const text = this.value;

    fetch(`/News/SearchPartial?searchText=${encodeURIComponent(text)}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('newsResult').innerHTML = html;
        });
});

function loadPage(page) {
    const text = document.getElementById('searchInput').value;

    fetch(`/News/SearchPartial?searchText=${encodeURIComponent(text)}&page=${page}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('newsResult').innerHTML = html;
        });
}