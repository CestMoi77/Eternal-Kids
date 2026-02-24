(function () {
    document.querySelectorAll('[data-step-next]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var current = btn.closest('.ek-step');
            var nextId = btn.getAttribute('data-step-next');
            if (!current || !nextId) return;
            current.classList.remove('active');
            var next = document.getElementById(nextId);
            if (next) next.classList.add('active');
        });
    });

    document.querySelectorAll('[data-step-prev]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var current = btn.closest('.ek-step');
            var prevId = btn.getAttribute('data-step-prev');
            if (!current || !prevId) return;
            current.classList.remove('active');
            var prev = document.getElementById(prevId);
            if (prev) prev.classList.add('active');
        });
    });
})();
