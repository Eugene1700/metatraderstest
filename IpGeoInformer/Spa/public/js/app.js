window.addEventListener('load', () => {
  const el = $('#app');

  const errorTemplate = Handlebars.compile($('#error-template').html());
  const ipTemplate = Handlebars.compile($('#ip-template').html());
  const locationTemplate = Handlebars.compile($('#location-template').html());
  const cityTemplate = Handlebars.compile($('#city-template').html());

  const api = axios.create({
    baseURL: 'http://localhost:5000/',
    timeout: 5000,
  });

  const router = new Router({
    mode: 'history',
    page404: (path) => {
      const html = errorTemplate({
        color: 'yellow',
        title: 'Error 404 - Page NOT Found!',
        message: `The path '/${path}' does not exist on this site`,
      });
      el.html(html);
    },
  });

  const showError = (error) => {
    const { title, message } = error.response.data;
    const html = errorTemplate({ color: 'red', title, message });
    el.html(html);
  };

  const getGeoIpResults = async () => {
    const ip = $('#ip').val();
    try {
      const response = await api.get(`/ip/location?ip=${ip}`);
      if (response.data) {
        const {latitude, longitude} = response.data;
        $('#result').html(`Широта: ${latitude}<br>Долгота: ${longitude}`);
        window.localStorage.setItem('ip-result', JSON.stringify({ip: ip, latitude: latitude, longitude: longitude}));
      }
      else {
        $('#result').html(`Локации не найдены`);
        window.localStorage.setItem('ip-result', JSON.stringify({ip: ip, latitude: null, longitude: null}));
      }
    } catch (error) {
      showError(error);
    } finally {
    }
  };

  const getPlacesByCityResults = async () => {
    const city = $('#city').val();
    try {
      const response = await api.get(`/city/locations?city=${city}`);
      const locations = response.data;
      if (locations && locations.length > 0) {
        const template = locationTemplate({locations: locations});
        $('#result').html(template);
        window.localStorage.setItem('city-result', JSON.stringify({city: city, locations: locations}));
      }
      else {
        $('#result').html("Локации не найдены");
        window.localStorage.setItem('city-result', JSON.stringify({city: city, locations: []}));
      }

    } catch (error) {
      showError(error);
    } finally {
    }
  };

  function validateIpAddress(ipaddress)
  {
    if (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(ipaddress))
    {
      return true;
    }
    return false;
  }

  const getGeoByIpHandler = () => {
    const ip = $('#ip').val();
    if (validateIpAddress(ip)) {
      getGeoIpResults();
      return false;
    }
    else {
      let r = errorTemplate({ message: "Некорректный ip-адрес"});
      $('#result').html(r);
    }
    return true;
  };

  const getPlacesByCityHandler = () => {
    getPlacesByCityResults();
    return true;
  };

  router.add('/', async () => {
    let html = ipTemplate();
    el.html(html);
    try {
      const r = JSON.parse(window.localStorage.getItem('ip-result'));
      if (r) {
        const {ip, latitude, longitude} = r;
        const ipFiled = $('#ip');
        ipFiled.val(ip);
        if (!latitude || !longitude) {
          $('#result').html(`Локации не найдены`);
        } else {
          $('#result').html(`Широта: ${latitude}<br>Долгота: ${longitude}`);
        }
        $('.submit').removeClass('disabledbutton');
      }
      else {
        $('.submit').addClass('disabledbutton');
      }

      $('.submit').click(getGeoByIpHandler);

      $('.text-field').on('input',function(e){
        if ($('.text-field').val()) {
          $('.submit').removeClass('disabledbutton');
        }
        else {
          $('.submit').addClass('disabledbutton');
        }
      });

      $('.text-field').inputmask("999.999.999.999");
    } catch (error) {
      showError(error);
    }
  });

  router.add('/city', async () => {
    let html = cityTemplate();
    el.html(html);
    try {
      const r = JSON.parse(window.localStorage.getItem('city-result'));
      if (r) {
        const {city, locations} = r;
        const cityField = $('#city');
        cityField.val(city);
        if (locations && locations.length > 0) {
          const template = locationTemplate({locations: locations});
          $('#result').html(template);
        }
        else {
          $('#result').html("Локации не найдены");
        }
        $('.submit').removeClass('disabledbutton');
      }
      else {
        $('.submit').addClass('disabledbutton');
      }

      $('.submit').click(getPlacesByCityHandler);
      $('.text-field').on('input',function(e){
        if ($('.text-field').val()) {
          $('.submit').removeClass('disabledbutton');
        }
        else {
          $('.submit').addClass('disabledbutton');
        }
      });
    } catch (error) {
      showError(error);
    }
  });

  router.navigateTo(window.location.pathname);

  // Highlight Active Menu on Load
  const link = $(`a[href$='${window.location.pathname}']`);
  link.addClass('active');

  $('a').on('click', (event) => {
    // Block page load
    event.preventDefault();

    // Highlight Active Menu on Click
    const target = $(event.target);
    $('.item').removeClass('active');
    target.addClass('active');

    // Navigate to clicked url
    const href = target.attr('href');
    const path = href.substr(href.lastIndexOf('/'));
    router.navigateTo(path);
  });
});
