document.addEventListener('DOMContentLoaded', function () {
    const roleSelect = document.getElementById('Role');
    const jobContainer = document.getElementById('jobContainer');

    function toggleJobDropdown() {
        const selectedRole = roleSelect.options[roleSelect.selectedIndex].text.toLowerCase();
        if (selectedRole === 'practitioner') {
            jobContainer.style.display = 'block';
        } else {
            jobContainer.style.display = 'none';
        }
    }

    roleSelect.addEventListener('change', toggleJobDropdown);
    toggleJobDropdown();
});