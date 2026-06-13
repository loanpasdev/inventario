// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
  if (window.jQuery && jQuery.validator) {
    jQuery.extend(jQuery.validator.messages, {
      required: "Este campo es obligatorio.",
      number: "Ingrese un numero valido.",
      maxlength: jQuery.validator.format("No puede exceder {0} caracteres."),
      minlength: jQuery.validator.format("Debe tener al menos {0} caracteres."),
      rangelength: jQuery.validator.format("Debe tener entre {0} y {1} caracteres."),
      range: jQuery.validator.format("Ingrese un valor entre {0} y {1}."),
      max: jQuery.validator.format("Ingrese un valor menor o igual a {0}."),
      min: jQuery.validator.format("Ingrese un valor mayor o igual a {0}.")
    });

    jQuery.validator.methods.number = function (value, element) {
      if (this.optional(element)) {
        return true;
      }

      var normalizedValue = (value || "").toString().replace(/\s/g, "").replace(",", ".");
      return /^-?(?:\d+|\d*\.\d+)$/.test(normalizedValue);
    };
  }

  var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
  tooltipTriggerList.forEach(function (tooltipTriggerEl) {
    new bootstrap.Tooltip(tooltipTriggerEl);
  });

  document.querySelectorAll("[data-password-toggle]").forEach(function (toggleButton) {
    var inputGroup = toggleButton.closest(".input-group");
    var passwordInput = inputGroup ? inputGroup.querySelector("[data-password-input]") : null;
    var hiddenIcon = toggleButton.querySelector('[data-icon-visible="hidden"]');
    var shownIcon = toggleButton.querySelector('[data-icon-visible="shown"]');
    var showLabel = toggleButton.getAttribute("data-show-label") || "Mostrar contraseña";
    var hideLabel = toggleButton.getAttribute("data-hide-label") || "Ocultar contraseña";

    if (!passwordInput) {
      return;
    }

    toggleButton.addEventListener("click", function () {
      var isPasswordHidden = passwordInput.getAttribute("type") === "password";

      passwordInput.setAttribute("type", isPasswordHidden ? "text" : "password");
      toggleButton.setAttribute("aria-label", isPasswordHidden ? hideLabel : showLabel);
      toggleButton.setAttribute("title", isPasswordHidden ? hideLabel : showLabel);

      if (hiddenIcon) {
        hiddenIcon.classList.toggle("d-none", isPasswordHidden);
      }

      if (shownIcon) {
        shownIcon.classList.toggle("d-none", !isPasswordHidden);
      }
    });
  });

  var filterGroups = document.querySelectorAll("[data-filter-group]");

  filterGroups.forEach(function (group) {
    var filters = group.querySelectorAll("[data-filter-target]");
    var searchInput = group.querySelector("[data-search-input]");
    var cards = group.querySelectorAll("[data-filter-card]");
    var emptyMessage = group.querySelector("[data-empty-message]");
    var countLabel = null;

    if (typeof group.closest === "function") {
      var cardContainer = group.closest(".card");
      if (cardContainer) {
        countLabel = cardContainer.querySelector("[data-filter-count]");
      }
    }

    if (!countLabel) {
      countLabel = group.querySelector("[data-filter-count]");
    }

    function normalize(value) {
      return (value || "").toString().trim().toLowerCase();
    }

    function getSelectedValues(filter) {
      return Array.from(filter.querySelectorAll('input[type="checkbox"]:checked'))
        .map(function (input) { return normalize(input.value); })
        .filter(function (value) { return value; });
    }

    function updateFilterLabel(filter) {
      var label = filter.querySelector("[data-filter-button-label]");
      var placeholder = filter.getAttribute("data-filter-placeholder") || "Todos";
      var checkedItems = Array.from(filter.querySelectorAll('input[type="checkbox"]:checked'));

      if (!label) {
        return;
      }

      if (checkedItems.length === 0) {
        label.textContent = placeholder;
        return;
      }

      if (checkedItems.length === 1) {
        label.textContent = checkedItems[0].getAttribute("data-label") || checkedItems[0].value;
        return;
      }

      label.textContent = checkedItems.length + " seleccionados";
    }

    function applyFilters() {
      var visibleCount = 0;

      cards.forEach(function (card) {
        var matches = true;
        var searchValue = searchInput ? normalize(searchInput.value) : "";
        var searchableContent = normalize(card.getAttribute("data-search"));

        filters.forEach(function (filter) {
          var target = filter.getAttribute("data-filter-target");
          var selectedValues = getSelectedValues(filter);
          var cardValue = normalize(card.getAttribute("data-" + target));

          if (selectedValues.length > 0 && selectedValues.indexOf(cardValue) === -1) {
            matches = false;
          }
        });        

        if (matches && searchValue && searchableContent.indexOf(searchValue) === -1) {
          matches = false;
        }

        card.classList.toggle("d-none", !matches);
        if (matches) {
          visibleCount += 1;
        }
      });

      if (emptyMessage) {
        emptyMessage.classList.toggle("d-none", visibleCount > 0);
      }

      if (countLabel) {
        var label = countLabel.getAttribute("data-count-label") || "item(s)";
        countLabel.textContent = visibleCount + " " + label;
      }
    }

    filters.forEach(function (filter) {
      updateFilterLabel(filter);

      filter.querySelectorAll('input[type="checkbox"]').forEach(function (input) {
        input.addEventListener("change", function () {
          updateFilterLabel(filter);
          applyFilters();
        });
      });
    });

    if (searchInput) {
      searchInput.addEventListener("input", applyFilters);
    }

    applyFilters();
  });

  if (typeof JsBarcode === "function") {
    document.querySelectorAll("[data-barcode]").forEach(function (element) {
      var value = (element.getAttribute("data-barcode") || "").toString().trim();

      if (!value) {
        return;
      }

      try {
        JsBarcode(element, value, {
          format: "CODE128",
          displayValue: false,
          margin: 0,
          marginTop: 0,
          marginBottom: 0,
          marginLeft: 0,
          marginRight: 0,
          background: "transparent",
          height: 44,
          width: 1.25,
          lineColor: "#111827"
        });
        element.setAttribute("preserveAspectRatio", "xMinYMin meet");

        try {
          var bbox = null;
          if (typeof element.getBBox === "function") {
            bbox = element.getBBox();
          }

          if (!bbox || !isFinite(bbox.width) || bbox.width <= 0) {
            var rects = element.querySelectorAll("rect");
            var minX = Infinity;
            var minY = Infinity;
            var maxX = -Infinity;
            var maxY = -Infinity;

            rects.forEach(function (rect) {
              var x = parseFloat(rect.getAttribute("x") || "0");
              var y = parseFloat(rect.getAttribute("y") || "0");
              var width = parseFloat(rect.getAttribute("width") || "0");
              var height = parseFloat(rect.getAttribute("height") || "0");

              if (!isFinite(x) || !isFinite(y) || !isFinite(width) || !isFinite(height) || width <= 0 || height <= 0) {
                return;
              }

              minX = Math.min(minX, x);
              minY = Math.min(minY, y);
              maxX = Math.max(maxX, x + width);
              maxY = Math.max(maxY, y + height);
            });

            if (isFinite(minX) && isFinite(minY) && isFinite(maxX) && isFinite(maxY) && maxX > minX && maxY > minY) {
              bbox = { x: minX, y: minY, width: maxX - minX, height: maxY - minY };
            }
          }

          if (bbox && isFinite(bbox.x) && isFinite(bbox.y) && isFinite(bbox.width) && isFinite(bbox.height) && bbox.width > 0 && bbox.height > 0) {
            element.setAttribute("viewBox", bbox.x + " " + bbox.y + " " + bbox.width + " " + bbox.height);
          }
        } catch {
        }
      } catch {
      }
    });
  }

  document.querySelectorAll(".alert.alert-dismissible").forEach(function (alert) {
    setTimeout(function () {
      var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
      bsAlert.close();
    }, 5000);
  });

  document.querySelectorAll("[data-status-switch]").forEach(function (toggle) {
    var card = toggle.closest(".card-body");
    var badge = card ? card.querySelector("[data-status-badge]") : null;

    if (!badge) {
      return;
    }

    toggle.addEventListener("click", function () {
      var isCurrentlyActive = toggle.classList.contains("is-active");
      var nextStatus = isCurrentlyActive ? "Inactivo" : "Activo";
      var isActive = nextStatus === "Activo";

      toggle.classList.toggle("is-active", isActive);
      toggle.classList.toggle("is-inactive", !isActive);
      toggle.setAttribute("aria-pressed", isActive ? "true" : "false");

      badge.textContent = nextStatus;
      badge.classList.toggle("im-status-badge--active", isActive);
      badge.classList.toggle("im-status-badge--inactive", !isActive);
    });
  });
});
