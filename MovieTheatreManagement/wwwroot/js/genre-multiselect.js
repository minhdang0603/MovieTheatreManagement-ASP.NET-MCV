/**
 * Genre Multiselect Component
 * 
 * A JavaScript module for creating a user-friendly genre multiselect dropdown
 * Supports searching, keyboard navigation, and tag-based selection
 */

$(document).ready(function () {
    // Initialize the genre multiselect component
    initGenreMultiselect();
});

function initGenreMultiselect() {
    const genreMultiselect = $('.genre-multiselect-container');
    const genreDropdown = $('.genre-dropdown');
    const genrePillsContainer = $('.genre-pills-container');
    const genreFilterInput = $('#genreFilterInput');
    const genreSearchInput = $('#genreSearchInput');

    // Toggle dropdown when clicking on the header
    $('.genre-multiselect-header').click(function (e) {
        if (!$(e.target).hasClass('genre-pill-remove')) {
            genreDropdown.toggleClass('show');
            genreFilterInput.focus();
        }
    });

    // Close dropdown when clicking outside
    $(document).on('click', function (e) {
        if (!genreMultiselect.is(e.target) && genreMultiselect.has(e.target).length === 0) {
            genreDropdown.removeClass('show');
        }
    });

    // Handle option selection
    $('.genre-option').click(function () {
        toggleGenreOption($(this));
    });

    // Remove pill when clicking on the remove button
    $(document).on('click', '.genre-pill-remove', function (e) {
        e.stopPropagation();
        const genreId = $(this).parent().data('genre-id');
        removeGenrePill(genreId);
    });

    // Filter options based on search input
    genreSearchInput.on('input', function () {
        const searchTerm = $(this).val().toLowerCase();
        filterGenreOptions(searchTerm);
    });

    // Use the same filter for the main input
    genreFilterInput.on('input', function () {
        const searchTerm = $(this).val().toLowerCase();
        genreSearchInput.val(searchTerm).trigger('input');
    });

    // Handle the clear all button
    $('#clearGenres').click(function (e) {
        e.stopPropagation();
        clearAllGenres();
    });

    // Handle the apply button
    $('#applyGenres').click(function (e) {
        e.stopPropagation();
        genreDropdown.removeClass('show');
    });

    // Focus input when clicking on pills container
    genrePillsContainer.click(function (e) {
        if ($(e.target).hasClass('genre-pills-container')) {
            genreFilterInput.focus();
        }
    });

    // Keyboard navigation
    genreFilterInput.on('keydown', function (e) {
        handleFilterKeyDown(e);
    });

    genreSearchInput.on('keydown', function (e) {
        handleSearchKeyDown(e);
    });

    // Set initial empty state class
    updateEmptyState();
}

/**
 * Toggles a genre option's selected state
 * @param {jQuery} option - The genre option element
 */
function toggleGenreOption(option) {
    const genreId = option.data('genre-id');
    const genreName = option.data('genre-name');
    const checkbox = option.find('input[type="checkbox"]');

    option.toggleClass('selected');

    if (option.hasClass('selected')) {
        checkbox.prop('checked', true);
        option.find('.genre-checkbox i').removeClass('bi-square').addClass('bi-check-square-fill');

        // Add pill if it doesn't exist
        if ($(`.genre-pill[data-genre-id="${genreId}"]`).length === 0) {
            addGenrePill(genreId, genreName);
        }
    } else {
        checkbox.prop('checked', false);
        option.find('.genre-checkbox i').removeClass('bi-check-square-fill').addClass('bi-square');

        // Remove pill
        $(`.genre-pill[data-genre-id="${genreId}"]`).remove();
    }

    // Update empty state
    updateEmptyState();
}

/**
 * Adds a genre pill to the header
 * @param {string|number} genreId - The genre ID
 * @param {string} genreName - The genre name
 */
function addGenrePill(genreId, genreName) {
    const pill = $(`
        <div class="genre-pill" data-genre-id="${genreId}">
            <span>${genreName}</span>
            <button type="button" class="genre-pill-remove">&times;</button>
        </div>
    `);
    $('.genre-pills-container').find('input').before(pill);
}

/**
 * Removes a genre pill and its corresponding option
 * @param {string|number} genreId - The genre ID to remove
 */
function removeGenrePill(genreId) {
    // Uncheck the option
    const option = $(`.genre-option[data-genre-id="${genreId}"]`);
    option.removeClass('selected');
    option.find('input[type="checkbox"]').prop('checked', false);
    option.find('.genre-checkbox i').removeClass('bi-check-square-fill').addClass('bi-square');

    // Remove the pill
    $(`.genre-pill[data-genre-id="${genreId}"]`).remove();

    // Update empty state
    updateEmptyState();
}

/**
 * Filters genre options based on search term
 * @param {string} searchTerm - The search term to filter by
 */
function filterGenreOptions(searchTerm) {
    $('.genre-option').each(function () {
        const genreName = $(this).data('genre-name').toLowerCase();
        if (genreName.includes(searchTerm)) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}

/**
 * Clears all selected genres
 */
function clearAllGenres() {
    // Uncheck all options
    $('.genre-option').removeClass('selected');
    $('.genre-option input[type="checkbox"]').prop('checked', false);
    $('.genre-option .genre-checkbox i').removeClass('bi-check-square-fill').addClass('bi-square');

    // Remove all pills
    $('.genre-pill').remove();

    // Update empty state
    updateEmptyState();
}

/**
 * Updates the empty state class on the header
 */
function updateEmptyState() {
    const header = $('.genre-multiselect-header');
    if ($('.genre-pill').length === 0) {
        header.addClass('empty');
    } else {
        header.removeClass('empty');
    }
}

/**
 * Handles keyboard navigation in the filter input
 * @param {Event} e - The keyboard event
 */
function handleFilterKeyDown(e) {
    const dropdown = $('.genre-dropdown');

    if (e.key === 'ArrowDown') {
        e.preventDefault();
        if (!dropdown.hasClass('show')) {
            dropdown.addClass('show');
        }
        $('#genreSearchInput').focus();
        focusFirstVisibleOption();
    } else if (e.key === 'Escape') {
        e.preventDefault();
        dropdown.removeClass('show');
    } else if (e.key === 'Backspace' && $('#genreFilterInput').val() === '') {
        // Remove the last pill when backspace is pressed and the input is empty
        const lastPill = $('.genre-pill').last();
        if (lastPill.length) {
            const genreId = lastPill.data('genre-id');
            removeGenrePill(genreId);
        }
    }
}

/**
 * Handles keyboard navigation in the search input
 * @param {Event} e - The keyboard event
 */
function handleSearchKeyDown(e) {
    if (e.key === 'ArrowDown') {
        e.preventDefault();
        focusFirstVisibleOption();
    } else if (e.key === 'Escape') {
        e.preventDefault();
        $('.genre-dropdown').removeClass('show');
        $('#genreFilterInput').focus();
    }
}

/**
 * Focuses the first visible option
 */
function focusFirstVisibleOption() {
    const firstVisibleOption = $('.genre-option:visible').first();
    if (firstVisibleOption.length) {
        firstVisibleOption.focus();
    }
}