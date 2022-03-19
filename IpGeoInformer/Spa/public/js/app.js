window.addEventListener('load', () => {
  const el = $('#app');

  // Compile Handlebar Templates
  const errorTemplate = Handlebars.compile($('#error-template').html());
  const ipTemplate = Handlebars.compile($('#ip-template').html());
  const locationTemplate = Handlebars.compile($('#location-template').html());
  const cityTemplate = Handlebars.compile($('#city-template').html());

  // Instantiate api handler
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

  // Display Error Banner
  const showError = (error) => {
    const { title, message } = error.response.data;
    const html = errorTemplate({ color: 'red', title, message });
    el.html(html);
  };

  const getGeoIpResults = async () => {
    const ip = $('#ip').val();
    try {
      const response = await api.get(`/ip/location?ip=${ip}`);
      const { latitude, longitude } = response.data;
      $('#result').html(`Широта: ${latitude} Долгота: ${longitude}`);
    } catch (error) {
      showError(error);
    } finally {
      $('#result-segment').removeClass('loading');
    }
  };

  const getPlacesByCityResults = async () => {
    const city = $('#city').val();
    try {
      const response = await api.get(`/city/locations?city=${city}`);
      const template = locationTemplate({locations: response.data});
      $('#result').html(template);
    } catch (error) {
      showError(error);
    } finally {
      $('#result-segment').removeClass('loading');
    }
  };

  const getGeoByIpHandler = () => {
    if ($('.ui.form').form('is valid')) {
      // hide error message
      $('.ui.error.message').hide();
      // Post to express server
      $('#result-segment').addClass('loading');
      getGeoIpResults();
      // Prevent page from submitting to server
      return false;
    }
    return true;
  };

  const getPlacesByCityHandler = () => {
    if ($('.ui.form').form('is valid')) {
      // hide error message
      $('.ui.error.message').hide();
      // Post to express server
      $('#result-segment').addClass('loading');
      getPlacesByCityResults();
      // Prevent page from submitting to server
      return false;
    }
    return true;
  };

  router.add('/', async () => {
    // Display loader first
    let html = ipTemplate();
    el.html(html);
    try {
      $('.loading').removeClass('loading');
      // Specify Form Validation Rules
      // $('.ui.form').form({
      //   fields: {
      //     amount: 'text',
      //   },
      // });
      // Specify Submit Handler
      $('.submit').click(getGeoByIpHandler);
    } catch (error) {
      showError(error);
    }
  });

  router.add('/city', async () => {
    // Display loader first
    let html = cityTemplate();
    el.html(html);
    try {

      $('.loading').removeClass('loading');
      // Specify Form Validation Rules
      // $('.ui.form').form({
      //   fields: {
      //     amount: 'text',
      //   },
      // });
      // Specify Submit Handler
      $('.submit').click(getPlacesByCityHandler);
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
